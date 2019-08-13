using System;
using ChustaSoft.Tools.ExecutionControl.Enums;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public interface IExecutionEventBusiness<TKey> where TKey : IComparable
    {
        bool Create(TKey executionId, ExecutionStatus status, string message);
    }
}