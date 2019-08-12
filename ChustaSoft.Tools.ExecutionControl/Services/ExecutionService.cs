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

        private readonly ExecutionControlConfiguration _executionControlConfiguration;

        private readonly IExecutionBusiness<TKey> _executionBusiness;
        private readonly IExecutionEventBusiness<TKey> _executionEventBusiness;


        public ExecutionService(ExecutionControlConfiguration executionControlConfiguration, IExecutionBusiness<TKey> executionBusiness, IExecutionEventBusiness<TKey> executionEventBusiness)
        {
            _executionControlConfiguration = executionControlConfiguration;

            _executionBusiness = executionBusiness;
            _executionEventBusiness = executionEventBusiness;
        }


        public void Execute<T>(TProcessEnum processName, Func<T> process)
        {
            var execution = PerformExecutionAttempt(processName);
            var availability = _executionBusiness.IsAllowed(execution);

            switch (availability)
            {
                case ExecutionAvailability.Abort:
                    PerformAbortExecution(execution);
                    break;
                case ExecutionAvailability.Block:
                    PerformBlockExecution(execution);
                    break;

                default:
                    PerformStartExecution(process, execution);
                    break;
            }
        }

        private void PerformAbortExecution(Execution<TKey> execution)
        {
            var executionId = _executionBusiness.Abort(execution.ProcessDefinitionId);

            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Aborted, $"Process aborted due to timeout exceeded: {_executionControlConfiguration.MinutesToAbort}");
        }

        private void PerformBlockExecution(Execution<TKey> execution)
        {
            _executionBusiness.Block(execution);

            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Aborted, $"Process blocked because another process is still running");
        }

        private Execution<TKey> PerformExecutionAttempt(TProcessEnum processName)
        {
            var execution = _executionBusiness.Register(processName.ToString());

            _executionEventBusiness.Create(execution.Id, ExecutionStatus.Waiting, "Process created waiting for execution");

            return execution;
        }

        private void PerformStartExecution<T>(Func<T> process, Execution<TKey> execution)
        {
            var processTask = new Task(() => process());
            processTask.RunSynchronously();

            switch (processTask.Status)
            {
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    _executionBusiness.Complete(execution, ExecutionResult.Error);
                    throw new ProcessExecutionException("Process execution failed", processTask.Exception);

                default:
                    _executionBusiness.Complete(execution, ExecutionResult.Success);
                    break;

            }
        }
    }
}
