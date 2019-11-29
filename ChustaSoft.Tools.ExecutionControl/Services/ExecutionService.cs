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
            return Task.Factory
                .StartNew(() => PerformStartAttempt(processName))
                .ContinueWith(task => PerformExecute(task.Result, process))
                .Result;
        }

        public TResult Execute<TResult>(TProcessEnum processName, Func<ExecutionContext<TKey>, TResult> process)
        {
            return Task.Factory
                .StartNew(() => PerformStartAttempt(processName))
                .ContinueWith(task => PerformExecute(task.Result, process))
                .Result;
        }

        #endregion


        #region Private methods

        private (Execution<TKey> Execution, ExecutionAvailability Availability) PerformStartAttempt(TProcessEnum processName)
        {
            var execution = PerformExecutionAttempt(processName);
            var availability = _executionBusiness.IsAllowed(execution);

            return (execution, availability);
        }

        private TResult PerformExecute<TResult>((Execution<TKey> Execution, ExecutionAvailability Availability) startAttempt, Func<TResult> process)
        {
            switch (startAttempt.Availability)
            {
                case ExecutionAvailability.Bypass:
                    PerformBypassExecution(process, startAttempt.Execution);
                    return default(TResult);
                case ExecutionAvailability.Block:
                    PerformBlockExecution(startAttempt.Execution);
                    return default(TResult);
                case ExecutionAvailability.Abort:
                    PerformAbortExecution(startAttempt.Execution);
                    return PerformStartExecution(process, startAttempt.Execution);

                default:
                    return PerformStartExecution(process, startAttempt.Execution);
            }
        }

        private TResult PerformExecute<TResult>((Execution<TKey> Execution, ExecutionAvailability Availability) startAttempt, Func<ExecutionContext<TKey>, TResult> process)
        {
            switch (startAttempt.Availability)
            {
                case ExecutionAvailability.Bypass:
                    PerformBypassExecution(process, startAttempt.Execution);
                    return default(TResult);
                case ExecutionAvailability.Block:
                    PerformBlockExecution(startAttempt.Execution);
                    return default(TResult);
                case ExecutionAvailability.Abort:
                    PerformAbortExecution(startAttempt.Execution);
                    return PerformStartExecution(process, startAttempt.Execution);

                default:
                    return PerformStartExecution(process, startAttempt.Execution);
            }
        }

        private void PerformBypassExecution<TResult>(Func<TResult> process, Execution<TKey> execution)
        {
            var previousExecution = _executionBusiness.GetPrevious(execution.ProcessDefinition.GetEnumDefinition<TProcessEnum>());

            if(previousExecution != null)
                _executionBusiness.Complete(previousExecution, ExecutionResult.Success);

            PerformRunProcess(process, execution);
        }

        private void PerformBypassExecution<TResult>(Func<ExecutionContext<TKey>, TResult> process,  Execution<TKey> execution)
        {
            var previousExecution = _executionBusiness.GetPrevious(execution.ProcessDefinition.GetEnumDefinition<TProcessEnum>());

            if (previousExecution != null)
                _executionBusiness.Complete(previousExecution, ExecutionResult.Success);

            PerformRunProcess(process, execution);
        }

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
            var execution = _executionBusiness.Register(processName);

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

        private void PerformRunProcess<TResult>(Func<TResult> process, Execution<TKey> execution)
        {
            var processTask = GetTask(process);

            PerformStartRegistration(execution);

            processTask.Start();
        }

        private void PerformRunProcess<TResult>(Func<ExecutionContext<TKey>, TResult> process, Execution<TKey> execution)
        {
            var processTask = GetTask(process, execution);

            PerformStartRegistration(execution);

            processTask.Start();
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
