using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public class ProcessDefinitionRepository<TKey> : RepositoryBase<TKey>, IProcessDefinitionRepository<TKey> where TKey : IComparable
    {

        protected ProcessDefinitionRepository(ExecutionControlContext<TKey> dbContext)
            : base(dbContext)
        { }


        public ProcessDefinition<TKey> Get(string processName)
        {
            return _dbContext.ProcessDefinitions
                .FirstOrDefault(x => x.Name == processName);
        }

        public IEnumerable<ProcessDefinition<TKey>> GetAll()
        {
            return _dbContext.ProcessDefinitions
                .ToList();
        }

    }
}
