using ChustaSoft.Tools.ExecutionControl.Entities;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IExecutionRepository<TKey> where TKey : IComparable
    {

        IEnumerable<Execution<TKey>> GetDaily(DateTime day);

        IEnumerable<Execution<TKey>> GetDaily<TProcessEnum>(TProcessEnum process, DateTime day) where TProcessEnum : struct, IConvertible;

        Execution<TKey> GetLast<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible;

        Execution<TKey> GetLastDead(Execution<TKey> currentExecution);

        Execution<TKey> GetLastCompleted(Execution<TKey> currentExecution);

        bool Save(Execution<TKey> execution);

        bool Update(Execution<TKey> previousExecution);

    }
}