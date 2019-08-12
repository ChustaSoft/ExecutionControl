using System;
using ChustaSoft.Tools.ExecutionControl.Contracts;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;

namespace ChustaSoft.Tools.ExecutionControl.Helpers
{
    public class ExecutionBuilder : IExecutionBuilder
    {

        public void Finish<TKey>(Execution<TKey> execution, ExecutionStatus status, ExecutionResult? result)
        {
            execution.EndDate = DateTime.Now;
            execution.Status = status;
            execution.Result = result;
        }

        public Execution<TKey> GetNew<TKey>(TKey definitionId, string server)
            => new Execution<TKey>
            {
                BeginDate = DateTime.Now,
                ProcessDefinitionId = definitionId,
                Status = ExecutionStatus.Running,
                Server = server
            };

    }
}
