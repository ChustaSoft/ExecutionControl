using System.Collections.Generic;
using System.Linq;


namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ProcessModuleDefinition<TKey>
    {

        public TKey Id { get; set; }

        public TKey ProcessDefinitionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Concurrent { get; set; }

        public bool Active { get; set; }


        public ProcessDefinition<TKey> ProcessDefinition { get; set; }

        public IEnumerable<ExecutionModule<TKey>> ExecutionModules { get; set; }


        public ProcessModuleDefinition()
        {
            ExecutionModules = Enumerable.Empty<ExecutionModule<TKey>>();
        }

    }
}
