using ChustaSoft.Tools.ExecutionControl.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ExecutionModule<TKey>
    {

        public TKey Id { get; set; }

        public TKey ExecutionId { get; set; }

        public TKey ModuleDefinitionId { get; set; }

        public ExecutionStatus Status { get; set; }

        public ExecutionResult? Result { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }


        public Execution<TKey> Execution { get; set; }

        public ProcessModuleDefinition<TKey> ModuleDefinition { get; set; }

        public IEnumerable<ExecutionModuleEvent<TKey>> ExecutionModuleEvents { get; set; }


        public ExecutionModule()
        {
            ExecutionModuleEvents = Enumerable.Empty<ExecutionModuleEvent<TKey>>();
        }

    }
}
