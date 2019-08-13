using ChustaSoft.Tools.ExecutionControl.Entities;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IExecutionRepository<TKey> where TKey : IComparable
    {
        Execution<TKey> GetLastBlocked(TKey processDefinitionId);

        Execution<TKey> GetLastCompleted(TKey processDefinitionId);

        bool Save(Execution<TKey> execution);

        bool Update(Execution<TKey> previousExecution);
    }
}