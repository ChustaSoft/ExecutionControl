using ChustaSoft.Tools.ExecutionControl.Configuration;
using ChustaSoft.Tools.ExecutionControl.Domain;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Exceptions;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public class ExecutionService<TKey, TProcessEnum> : IExecutionService<TKey, TProcessEnum> 
            where TKey : IComparable
            where TProcessEnum : struct, IConvertible 
    {

        private readonly ExecutionControlConfiguration _executionControlConfiguration;

        private readonly IExecutionBusiness<TKey> _executionBusiness;
        private readonly IExecutionEventBusiness<TKey> _executionEventBusiness;


        public ExecutionService(ExecutionControlConfiguration executionControlConfiguration, IExecutionBusiness<TKey> executionBusiness, IExecutionEventBusiness<TKey> executionEventBusiness)
        {
            _executionControlConfiguration = executionControlConfiguration;

            _executionBusiness = executionBusiness;
            _executionEventBusiness = executionEventBusiness;
        }


        public TKey Execute<T>(TProcessEnum processName, Func<T> process)
        {
            var execution = PerformExecutionAttempt(processName);
            var availability = _executionBusiness.IsAllowed(execution);

            switch (availability)
            {
                case ExecutionAvailability.Block:
                    PerformBlockExecution(execution);
                    break;
                case ExecutionAvailability.Abort:
                    PerformAbortExecution(execution);
                    PerformStartExecution(process, execution);
                    break;

                default:
                    PerformStartExecution(process, execution);
                    break;
            }
            return execution.Id;
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
            var execution = _executionBusiness.Register(processName.ToString());

            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Waiting, "Process created waiting for execution");

            return execution;
        }

        private void PerformStartExecution<T>(Func<T> process, Execution<TKey> execution)
        {
            var processTask = RunProcess(process, execution);

            FinishProcess(execution, processTask);
        }

        private Task<T> RunProcess<T>(Func<T> process, Execution<TKey> execution)
        {
            var processTask = new Task<T>(() => process());
            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Running, $"Process execution in progress");

            processTask.RunSynchronously();
            return processTask;
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

    }
}
