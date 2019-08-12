using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;
using System;


namespace ChustaSoft.Tools.ExecutionControl.Contracts
{
    public interface IExecutionBuilder
    {

        Execution<TKey> GetNew<TKey>(TKey definitionId, string server);

        void Finish<TKey>(Execution<TKey> execution, ExecutionStatus status, ExecutionResult? result);

    }
}
