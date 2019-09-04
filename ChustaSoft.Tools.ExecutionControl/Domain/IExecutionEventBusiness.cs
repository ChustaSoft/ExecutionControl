using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public interface IExecutionEventBusiness<TKey> where TKey : IComparable
    {

        bool Create(TKey executionId, ExecutionStatus status, string message);

        bool Create(ExecutionEventArgs<TKey> executionEventArgs);

    }
}