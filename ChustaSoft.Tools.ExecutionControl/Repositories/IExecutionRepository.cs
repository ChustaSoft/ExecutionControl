using ChustaSoft.Tools.ExecutionControl.Entities;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IExecutionRepository<TKey> where TKey : IComparable
    {
        Execution<TKey> GetLastDead(Execution<TKey> currentExecution);

        Execution<TKey> GetLastCompleted(Execution<TKey> currentExecution);

        bool Save(Execution<TKey> execution);

        bool Update(Execution<TKey> previousExecution);
    }
}