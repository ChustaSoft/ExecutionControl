using ChustaSoft.Tools.ExecutionControl.Model;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Helpers
{
    public class ExecutionDefinitionBuilder<TKey, TProcessEnum>
    {

        private IDictionary<TProcessEnum, ProcessContextDefinition<TKey>> _internalDictionary;


        public ExecutionDefinitionBuilder()
        {
            _internalDictionary = new Dictionary<TProcessEnum, ProcessContextDefinition<TKey>>();
        }


        public ExecutionDefinitionBuilder<TKey, TProcessEnum> Add(TProcessEnum process, ProcessContextDefinition<TKey> processContextDefinition)
        {
            _internalDictionary.TryAdd(process, processContextDefinition);

            return this;
        }

        public IDictionary<TProcessEnum, ProcessContextDefinition<TKey>> Build() => _internalDictionary;

    }
}
