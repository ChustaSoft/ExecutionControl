using ChustaSoft.Tools.ExecutionControl.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Entities
{
    public class Execution<TKey> where TKey : IComparable
    {

        public TKey Id { get; set; }
        public TKey ProcessDefinitionId { get; set; }
        public ExecutionStatus Status { get; set; }
        public ExecutionResult Result { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Host { get; set; }

        public ProcessDefinition<TKey> ProcessDefinition { get; set; }
        public ICollection<ExecutionEvent<TKey>> ExecutionEvents { get; set; }


        public Execution()
        {
            ExecutionEvents = Enumerable.Empty<ExecutionEvent<TKey>>().ToList();
        }

    }
}
