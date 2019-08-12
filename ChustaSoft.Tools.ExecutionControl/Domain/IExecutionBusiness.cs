using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public interface IExecutionBusiness<TKey> where TKey : IComparable
    {

        Execution<TKey> Register(string processName);

        TKey Abort(TKey processDefinitionId);

        TKey Block(Execution<TKey> execution);

        TKey Complete(Execution<TKey> execution, ExecutionResult result);

        ExecutionAvailability IsAllowed(Execution<TKey> execution);

    }
}