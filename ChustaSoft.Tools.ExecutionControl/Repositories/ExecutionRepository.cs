using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public class ExecutionRepository<TKey> : RepositoryBase<TKey>, IExecutionRepository<TKey> where TKey : IComparable
    {

        public ExecutionRepository(ExecutionControlContext<TKey> dbContext)
            : base(dbContext)
        { }


        public IEnumerable<Execution<TKey>> GetDaily(DateTime day)
        {
            return _dbContext.Executions.Include(x => x.ExecutionEvents).Include(x => x.ProcessDefinition)
                .Where(x => x.BeginDate.DayOfYear == day.DayOfYear && x.BeginDate.Year == day.Year)
                .OrderByDescending(x => x.BeginDate)
                .ToList();
        }

        public IEnumerable<Execution<TKey>> GetDaily<TProcessEnum>(TProcessEnum process, DateTime day) where TProcessEnum : struct, IConvertible
        {
            return _dbContext.Executions.Include(x => x.ExecutionEvents).Include(x => x.ProcessDefinition)
                .Where(x => x.ProcessDefinition.Name == process.ToString() && x.BeginDate.DayOfYear == day.DayOfYear && x.BeginDate.Year == day.Year)
                .OrderByDescending(x => x.BeginDate)
                .ToList();
        }

        public Execution<TKey> GetLast<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible
        {
            return _dbContext.Executions.Include(x => x.ExecutionEvents).Include(x => x.ProcessDefinition)
                .OrderByDescending(x => x.BeginDate)
                .First(x => x.ProcessDefinition.Name == process.ToString());
        }

        public Execution<TKey> GetLastDead(Execution<TKey> currentExecution)
        {
            return _dbContext.Executions
               .Where(x => x.ProcessDefinitionId.Equals(currentExecution.ProcessDefinitionId) && !x.Id.Equals(currentExecution.Id) && (x.Status == Enums.ExecutionStatus.Waiting || x.Status == Enums.ExecutionStatus.Running))
               .OrderByDescending(x => x.BeginDate)
               .First();
        }

        public Execution<TKey> GetLastCompleted(Execution<TKey> currentExecution)
        {
            return _dbContext.Executions
               .Where(x => x.ProcessDefinitionId.Equals(currentExecution.ProcessDefinitionId) && !x.Id.Equals(currentExecution.Id) && x.Status != Enums.ExecutionStatus.Aborted && x.Status != Enums.ExecutionStatus.Blocked)
               .OrderByDescending(x => x.BeginDate)
               .FirstOrDefault();
        }

        public bool Save(Execution<TKey> execution)
        {
            _dbContext.Executions.Add(execution);

            return _dbContext.SaveChanges() > 0;
        }

        public bool Update(Execution<TKey> execution)
        {
            _dbContext.Executions.Update(execution);

            return _dbContext.SaveChanges() > 0;
        }

    }
}
