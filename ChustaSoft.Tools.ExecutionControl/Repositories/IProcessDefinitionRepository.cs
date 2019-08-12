using ChustaSoft.Tools.ExecutionControl.Entities;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IProcessDefinitionRepository<TKey> where TKey : IComparable
    {
        IEnumerable<ProcessDefinition<TKey>> GetAll();

        ProcessDefinition<TKey> Get(string processName);
    }
}