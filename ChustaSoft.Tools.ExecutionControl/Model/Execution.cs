using ChustaSoft.Tools.ExecutionControl.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class Execution<TKey>
    {

        public TKey Id { get; set; }

        public TKey ProcessDefinitionId { get; set; }

        public ExecutionStatus Status { get; set; }

        public ExecutionResult? Result { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Server { get; set; }


        public ProcessDefinition<TKey> ProcessDefinition { get; set; }

        public IEnumerable<ExecutionEvent<TKey>> ExecutionEvents { get; set; }

        public IEnumerable<ExecutionModule<TKey>> ExecutionModules { get; set; }


        public Execution()
        {
            ExecutionEvents = Enumerable.Empty<ExecutionEvent<TKey>>();
            ExecutionModules = Enumerable.Empty<ExecutionModule<TKey>>();
        }

    }
}
