using ChustaSoft.Tools.ExecutionControl.Enums;
using System.Collections.Generic;
using System.Linq;


namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ProcessDefinition<TKey>
    {

        public TKey Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ExecutionType Type { get; set; }

        public bool Active { get; set; }


        public ICollection<Execution<TKey>> Executions { get; set; }

        public ICollection<ProcessModuleDefinition<TKey>> ModuleDefinitions { get; set; }


        public ProcessDefinition()
        {
            Executions = Enumerable.Empty<Execution<TKey>>().ToList();
            ModuleDefinitions = Enumerable.Empty<ProcessModuleDefinition<TKey>>().ToList();
        }

    }
}
