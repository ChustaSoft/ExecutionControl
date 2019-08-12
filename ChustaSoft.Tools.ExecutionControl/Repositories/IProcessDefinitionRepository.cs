using ChustaSoft.Tools.ExecutionControl.Model;
using System.Collections.Generic;


namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IProcessDefinitionRepository<TKey>
    {

        bool Save(ProcessDefinition<TKey> definition);

        bool Save(IEnumerable<ProcessDefinition<TKey>> definitions);

    }
}
