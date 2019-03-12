using ChustaSoft.Common.Helpers;
using ChustaSoft.Common.Utilities;
using ChustaSoft.Tools.ExecutionControl.Contracts;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;
using System.Collections.Generic;
using System.Linq;


namespace ChustaSoft.Tools.ExecutionControl.Helpers
{
    public class ProcessDefinitionBuilder<TKey> : IProcessDefinitionBuilder<TKey>, IBuilder<IEnumerable<ProcessDefinition<TKey>>>
    {

        private ICollection<ProcessDefinition<TKey>> _executionDefinitionCollection;
        private ProcessDefinition<TKey> _executionDefinition;
        private ProcessModuleDefinition<TKey> _processModuleDefinition;


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

        public IProcessDefinitionBuilder<TKey> AddModule(string name, string description, bool concurrent = false)
        {
            TryAddCurrentModule();

            _processModuleDefinition = new ProcessModuleDefinition<TKey>();

            _processModuleDefinition.Name = name;
            _processModuleDefinition.Description = description;
            _processModuleDefinition.Concurrent = concurrent;

            return this;
        }

        public IEnumerable<ProcessDefinition<TKey>> Build()
        {
            TryAddCurrentElement();

            return _executionDefinitionCollection;
        } 

        private void TryAddCurrentElement()
        {
            TryAddCurrentModule();

            if (_executionDefinition != null)
                _executionDefinitionCollection.Add(_executionDefinition);

            _executionDefinition = null;
        }

        private void TryAddCurrentModule()
        {
            if (_processModuleDefinition != null)
                _executionDefinition.ModuleDefinitions.Add(_processModuleDefinition);

            _processModuleDefinition = null;
        }

    }
}
