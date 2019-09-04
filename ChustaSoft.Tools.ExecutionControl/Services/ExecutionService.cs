using ChustaSoft.Tools.ExecutionControl.Configuration;
using ChustaSoft.Tools.ExecutionControl.Domain;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Exceptions;
using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public class ExecutionService<TKey, TProcessEnum> : IExecutionService<TKey, TProcessEnum> 
            where TKey : IComparable
            where TProcessEnum : struct, IConvertible 
    {

        #region Fields

        private readonly ExecutionControlConfiguration _executionControlConfiguration;

        private readonly IExecutionBusiness<TKey> _executionBusiness;
        private readonly IExecutionEventBusiness<TKey> _executionEventBusiness;

        #endregion


        #region Constructor

        public ExecutionService(ExecutionControlConfiguration executionControlConfiguration, IExecutionBusiness<TKey> executionBusiness, IExecutionEventBusiness<TKey> executionEventBusiness)
        {
            _executionControlConfiguration = executionControlConfiguration;

            _executionBusiness = executionBusiness;
            _executionEventBusiness = executionEventBusiness;
        }

        #endregion


        #region Public methods

        public TResult Execute<TResult>(TProcessEnum processName, Func<TResult> process)
        {
            var execution = PerformExecutionAttempt(processName);
            var availability = _executionBusiness.IsAllowed(execution);

            switch (availability)
            {
                case ExecutionAvailability.Block:
                    PerformBlockExecution(execution);
                    return default(TResult);
                case ExecutionAvailability.Abort:
                    PerformAbortExecution(execution);
                    return PerformStartExecution(process, execution);

                default:
                    return PerformStartExecution(process, execution);
            }
        }

        public TResult Execute<TResult>(TProcessEnum processName, Func<ExecutionContext<TKey>, TResult> process)
        {
            var execution = PerformExecutionAttempt(processName);
            var availability = _executionBusiness.IsAllowed(execution);

            switch (availability)
            {
                case ExecutionAvailability.Block:
                    PerformBlockExecution(execution);
                    return default(TResult);
                case ExecutionAvailability.Abort:
                    PerformAbortExecution(execution);
                    return PerformStartExecution(process, execution);

                default:
                    return PerformStartExecution(process, execution);
            }
        }

        #endregion


        #region Private methods

        private void PerformAbortExecution(Execution<TKey> execution)
        {
            var executionId = _executionBusiness.Abort(execution);

            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Aborted, $"Process aborted due to timeout exceeded: {_executionControlConfiguration.MinutesToAbort}");
        }

        private void PerformBlockExecution(Execution<TKey> execution)
        {
            _executionBusiness.Block(execution);

            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Blocked, $"Process blocked because another process is still running");
        }

        private Execution<TKey> PerformExecutionAttempt(TProcessEnum processName)
        {
            var execution = _executionBusiness.Register(processName.ToString());

            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Waiting, "Process created waiting for execution");

            return execution;
        }

        private TResult PerformStartExecution<TResult>(Func<TResult> process, Execution<TKey> execution)
        {
            var processTask = RunProcess(process, execution);

            return FinishProcess(execution, processTask);
        }

        private TResult PerformStartExecution<TResult>(Func<ExecutionContext<TKey>, TResult> process, Execution<TKey> execution)
        {
            var processTask = RunProcess(process, execution);

            return FinishProcess(execution, processTask);
        }

        private Task<TResult> RunProcess<TResult>(Func<TResult> process, Execution<TKey> execution)
        {
            var processTask = GetTask(process);

            PerformStartRegistration(execution);

            processTask.RunSynchronously();

            return processTask;
        }

        private Task<TResult> RunProcess<TResult>(Func<ExecutionContext<TKey>, TResult> process, Execution<TKey> execution)
        {
            var processTask = GetTask(process, execution);

            PerformStartRegistration(execution);

            processTask.RunSynchronously();

            return processTask;
        }

        private Task<TResult> GetTask<TResult>(Func<TResult> process)
        {
            return new Task<TResult>(() => process());
        }

        private Task<TResult> GetTask<TResult>(Func<ExecutionContext<TKey>, TResult> process, Execution<TKey> execution)
        {
            var executionContext = CreateManagedContext(execution);
            var processTask = new Task<TResult>(() => process(executionContext));

            return processTask;
        }

        private ExecutionContext<TKey> CreateManagedContext(Execution<TKey> execution)
        {
            var executionContext = new ExecutionContext<TKey>(execution.Id);

            executionContext.Checkpoint += ExecutionContext_Checkpoint;

            return executionContext;
        }

        private void PerformStartRegistration(Execution<TKey> execution)
        {
            _executionBusiness.Start(execution);
            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Running, $"Process execution in progress");
        }

        private T FinishProcess<T>(Execution<TKey> execution, Task<T> processTask)
        {
            switch (processTask.Status)
            {
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    _executionBusiness.Complete(execution, ExecutionResult.Error);
                    _executionEventBusiness.Create(execution.Id, ExecutionStatus.Finished, $"Process finished with errors: {processTask.Exception?.Message ?? string.Empty}");
                    throw new ProcessExecutionException("Process execution failed", processTask.Exception);

                default:
                    _executionBusiness.Complete(execution, ExecutionResult.Success);
                    _executionEventBusiness.Create(execution.Id, ExecutionStatus.Finished, "Process finished without errors");
                    break;
            }

            return processTask.Result;
        }

        private void ExecutionContext_Checkpoint(object sender, ExecutionEventArgs<TKey> args)
            => _executionEventBusiness.Create(args);

        #endregion

    }
}
