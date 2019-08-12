using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public class ExecutionService<TKey, TProcessEnum> : IExecutionService<TKey, TProcessEnum> 
        where TKey : IComparable
        where TProcessEnum : struct, IConvertible 
    {

        private readonly IExecutionBusiness<TKey> _executionBusiness;


        public ExecutionService(IExecutionBusiness<TKey> executionBusiness)
        {
            _executionBusiness = executionBusiness;
        }


        public void Execute<T>(TProcessEnum processName, Func<T> process)
        {
            var execution = _executionBusiness.Register(processName.ToString());
            var availability = _executionBusiness.IsAllowed(execution);

            switch (availability)
            {
                case ExecutionAvailability.Abort:
                    _executionBusiness.Abort(execution.ProcessDefinitionId);
                    break;
                case ExecutionAvailability.Block:
                    _executionBusiness.Block(execution);
                    break;

                default:
                    PerformExecution(process, execution);
                    break;
            }
        }


        private void PerformExecution<T>(Func<T> process, Execution<TKey> execution)
        {
            var processTask = new Task(() => process());
            processTask.RunSynchronously();

            switch (processTask.Status)
            {
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    _executionBusiness.Complete(execution, ExecutionResult.Error);
                    break;

                default:
                    _executionBusiness.Complete(execution, ExecutionResult.Success);
                    break;

            }
        }
    }
}
