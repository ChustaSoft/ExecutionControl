using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Entities;
using System;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public class ExecutionRepository<TKey> : RepositoryBase<TKey>, IExecutionRepository<TKey> where TKey : IComparable
    {

        public ExecutionRepository(ExecutionControlContext<TKey> dbContext)
            : base(dbContext)
        { }


        public Execution<TKey> GetLastBlocked(TKey processDefinitionId)
        {
            return _dbContext.Executions
               .Where(x => x.ProcessDefinitionId.Equals(processDefinitionId)  && (x.Status == Enums.ExecutionStatus.Waiting || x.Status == Enums.ExecutionStatus.Running))
               .OrderByDescending(x => x.BeginDate)
               .First();
        }

        public Execution<TKey> GetLastCompleted(TKey processDefinitionId)
        {
            return _dbContext.Executions
               .Where(x => x.ProcessDefinitionId.Equals(processDefinitionId) && x.Status != Enums.ExecutionStatus.Waiting && x.Status != Enums.ExecutionStatus.Aborted)
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
