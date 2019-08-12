using ChustaSoft.Tools.ExecutionControl.Entities;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IExecutionRepository<TKey> where TKey : IComparable
    {
        Execution<TKey> GetLastBlocked(TKey processDefinitionId);

        Execution<TKey> GetLastCompleted(TKey processDefinitionId);

        void Save(Execution<TKey> execution);

        void Update(Execution<TKey> previousExecution);
    }
}