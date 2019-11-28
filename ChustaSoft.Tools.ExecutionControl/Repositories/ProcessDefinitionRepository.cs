using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public class ProcessDefinitionRepository<TKey> : RepositoryBase<TKey>, IProcessDefinitionRepository<TKey> 
        where TKey : IComparable
    {

        public ProcessDefinitionRepository(ExecutionControlContext<TKey> dbContext)
            : base(dbContext)
        { }


        public ProcessDefinition<TKey> Get<TProcessEnum>(TProcessEnum processName) 
            where TProcessEnum : struct, IConvertible
        {
            return _dbContext.ProcessDefinitions
                .First(x => x.Name == processName.ToString());
        }

        public IEnumerable<ProcessDefinition<TKey>> GetAll()
        {
            return _dbContext.ProcessDefinitions
                .ToList();
        }

        public bool Save(ProcessDefinition<TKey> processDefinition)
        {
            _dbContext.Add(processDefinition);

            return _dbContext.SaveChanges() > 0;
        }

        public bool Update(ProcessDefinition<TKey> processDefinition)
        {
            _dbContext.Update(processDefinition);

            return _dbContext.SaveChanges() > 0;
        }

    }
}
