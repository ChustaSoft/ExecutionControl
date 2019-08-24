using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Entities
{
    public class ProcessDefinition<TKey> where TKey : IComparable
    {

        public TKey Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public ICollection<Execution<TKey>> Executions { get; set; }


        public ProcessDefinition()
        {
            Executions = Enumerable.Empty<Execution<TKey>>().ToList();
        }

    }
}
