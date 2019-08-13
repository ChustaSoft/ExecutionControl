using ChustaSoft.Tools.ExecutionControl.Context;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public class RepositoryBase<TKey> where TKey : IComparable
    {

        protected readonly ExecutionControlContext<TKey> _dbContext;


        protected RepositoryBase(ExecutionControlContext<TKey> dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
