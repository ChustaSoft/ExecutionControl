using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.ExecutionControl.Attributes;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Helpers
{
    public class ProcessDefinitionBuilder<TKey, TProcessEnum>
        where TKey : IComparable
        where TProcessEnum : struct, IConvertible
    {

        public ICollection<ProcessDefinition<TKey>> Definitions { get; }

        public int Count => Definitions.Count;


        public ProcessDefinitionBuilder()
        {
            Definitions = new List<ProcessDefinition<TKey>>();
        }


        public void Add(TProcessEnum process)
        {
            var definition = CreateDefinition(process);

            Definitions.Add(definition);
        }

        public void Autogenerate()
        {
            foreach (var processType in EnumsHelper.GetEnumList<TProcessEnum>())
                Add(processType);
        }


        private ProcessDefinition<TKey> CreateDefinition(TProcessEnum process)
            => new ProcessDefinition<TKey>
            {
                Active = true,
                Name = process.ToString(),
                Description = (process as Enum).GetDescription(),
                Type = GetProcessType(process)
            };

        private ProcessType GetProcessType(TProcessEnum process)
        {
            var processAttributes = (process as Enum).GetAttributes<ProcessDefinitionAttribute>();

            if (processAttributes != null)
                return processAttributes.Type;

            return ProcessType.Background;
        }

    }
}
