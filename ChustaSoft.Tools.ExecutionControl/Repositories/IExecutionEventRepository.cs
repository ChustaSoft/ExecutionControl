using System;
using ChustaSoft.Tools.ExecutionControl.Entities;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IExecutionEventRepository<TKey> where TKey : IComparable
    {
        bool Create(ExecutionEvent<TKey> executionEvent);
    }
}