using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public interface IExecutionBusiness<TKey> where TKey : IComparable
    {

        Execution<TKey> Register(string processName);

        TKey Abort(Execution<TKey> currentExecution);

        TKey Block(Execution<TKey> currentExecution);

        TKey Complete(Execution<TKey> execution, ExecutionResult result);

        ExecutionAvailability IsAllowed(Execution<TKey> execution);

    }
}