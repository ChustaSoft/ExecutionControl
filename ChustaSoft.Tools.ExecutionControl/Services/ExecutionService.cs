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
                case Enums.ExecutionAvailability.Abort:
                    _executionBusiness.Abort(execution.ProcessDefinitionId);
                    break;
                case Enums.ExecutionAvailability.Block:
                    _executionBusiness.Block(execution);
                    break;

                default:
                    PerformExecution(process, execution);
                    break;
            }
        }


        private void PerformExecution<T>(Func<T> process, object execution)
        {
            var tmpTaskToExecute = new Task(() => process());
            tmpTaskToExecute.RunSynchronously();

            switch (tmpTaskToExecute.Status)
            {
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    //PerformUnsuccessfulEnd(process, tmpLastRun, "Error executing Synchronously task");
                    //throw new ProcessExecutionException("Error executing Synchronously task", tmpTaskToExecute.Exception);

                default:
                    //PerformSuccessfulEnd(process, tmpLastRun, "Error executing Synchronously task");
                    break;

            }
        }
    }
}
