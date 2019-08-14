using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Entities;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public class ExecutionEventRepository<TKey> : RepositoryBase<TKey>, IExecutionEventRepository<TKey> where TKey : IComparable
    {

        public ExecutionEventRepository(ExecutionControlContext<TKey> dbContext)
           : base(dbContext)
        { }


        public bool Create(ExecutionEvent<TKey> executionEvent)
        {
            _dbContext.Add(executionEvent);

            return _dbContext.SaveChanges() > 0;
        }

    }
}
