using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public interface IExecutionBusiness<TKey> where TKey : IComparable
    {

        Execution<TKey> GetPrevious<TProcessEnum>(TProcessEnum processName) where TProcessEnum : struct, IConvertible;

        Execution<TKey> Register<TProcessEnum>(TProcessEnum processName) where TProcessEnum : struct, IConvertible;

        TKey Start(Execution<TKey> currentExecution);

        TKey Abort(Execution<TKey> currentExecution);

        TKey Block(Execution<TKey> currentExecution);

        TKey Complete(Execution<TKey> execution, ExecutionResult result);

        ExecutionAvailability IsAllowed(Execution<TKey> execution);

    }
}