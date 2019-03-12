using ChustaSoft.Common.Utilities;
using ChustaSoft.Tools.ExecutionControl.Contracts;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;
using System.Collections.Generic;
using System.Linq;


namespace ChustaSoft.Tools.ExecutionControl.Helpers
{
    public class ProcessDefinitionBuilder<TKey> : IProcessDefinitionBuilder<TKey>, IMultipleBuilder<ProcessDefinition<TKey>>, IInternalParentBuilder<IProcessDefinitionBuilder<TKey>, ProcessModuleDefinition<TKey>>
    {

        private ICollection<ProcessDefinition<TKey>> _executionDefinitionCollection;
        private ProcessDefinition<TKey> _executionDefinition;


        public ICollection<ErrorMessage> Errors { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


        public ProcessDefinitionBuilder()
        {
            _executionDefinitionCollection = Enumerable.Empty<ProcessDefinition<TKey>>().ToList();
        }

        public IProcessDefinitionBuilder<TKey> New(string name, string description)
        {
            TryAddCurrentElement();

            _executionDefinition = new ProcessDefinition<TKey>
            {
                Name = name,
                Description = description
            };

            return this;
        }

        public IProcessDefinitionBuilder<TKey> SetType(ExecutionType type)
        {
            _executionDefinition.Type = type;

            return this;
        }

        public IProcessModuleDefinitionBuilder<TKey> AddModule(string name, string description)
        {
            var _processModuleDefinitionBuilder = new ProcessModuleDefinitionBuilder<TKey>(this);

            _processModuleDefinitionBuilder.New(name, description);

            return _processModuleDefinitionBuilder;
        }

        public IProcessDefinitionBuilder<TKey> Integrate(ProcessModuleDefinition<TKey> data)
        {
            _executionDefinition.ModuleDefinitions.Add(data);

            return this;
        }

        public ProcessDefinition<TKey> Build()
        {
            return _executionDefinition;
        }

        public IEnumerable<ProcessDefinition<TKey>> BuildAll()
        {
            TryAddCurrentElement();

            return _executionDefinitionCollection;
        } 

        private void TryAddCurrentElement()
        {
            if (_executionDefinition != null)
            {
                _executionDefinitionCollection.Add(_executionDefinition);
            }
        }

    }
}
