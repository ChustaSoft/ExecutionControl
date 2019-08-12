using System;
using System.Collections.Generic;
using ChustaSoft.Tools.ExecutionControl.Entities;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IProcessDefinitionRepository<TKey> where TKey : IComparable
    {
        IEnumerable<ProcessDefinition<TKey>> GetAll();

        ProcessDefinition<TKey> Get(string processName);
    }
}