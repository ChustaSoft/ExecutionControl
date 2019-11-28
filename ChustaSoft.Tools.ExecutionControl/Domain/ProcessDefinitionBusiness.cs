using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Helpers;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public class ProcessDefinitionBusiness<TKey, TProcessEnum> : IProcessDefinitionBusiness<TKey, TProcessEnum>
            where TKey : IComparable
            where TProcessEnum : struct, IConvertible
    {

        #region Fields

        private readonly IProcessDefinitionRepository<TKey> _processDefinitionRepository;

        #endregion


        #region Constructor

        public ProcessDefinitionBusiness(IProcessDefinitionRepository<TKey> processDefinitionRepository)
        {
            _processDefinitionRepository = processDefinitionRepository;
        }

        #endregion


        #region Public methods

        public ProcessDefinition<TKey> Get(TProcessEnum processEnum)
        {
            return _processDefinitionRepository.Get(processEnum);
        }

        public void Setup()
        {
            var existingDefinitions = _processDefinitionRepository.GetAll();
            var createdDefinitions = GetDefinitions();

            CheckExistingDefinitions(existingDefinitions, createdDefinitions);
            CreateNewDefinitions(createdDefinitions);
        }

        #endregion


        #region Private methods

        private void CreateNewDefinitions(ICollection<ProcessDefinition<TKey>> createdDefinitions)
        {
            foreach (var newDefinition in createdDefinitions)
                _processDefinitionRepository.Save(newDefinition);
        }

        private void CheckExistingDefinitions(IEnumerable<ProcessDefinition<TKey>> existingDefinitions, ICollection<ProcessDefinition<TKey>> createdDefinitions)
        {
            foreach (var definition in existingDefinitions)
                if (createdDefinitions.Contains(definition) && !definition.Active)
                    ReActivateDefinition(createdDefinitions, definition);
                else if (!createdDefinitions.Contains(definition))
                    DeprecateDefinition(definition);
                else if (createdDefinitions.Contains(definition) && definition.Active)
                    RemoveExistingDefinition(createdDefinitions, definition);
        }

        private void DeprecateDefinition(ProcessDefinition<TKey> definition)
        {
            definition.Active = false;
            _processDefinitionRepository.Update(definition);
        }

        private void ReActivateDefinition(ICollection<ProcessDefinition<TKey>> createdDefinitions, ProcessDefinition<TKey> definition)
        {
            definition.Active = true;
            _processDefinitionRepository.Update(definition);
            RemoveExistingDefinition(createdDefinitions, definition);
        }

        private void RemoveExistingDefinition(ICollection<ProcessDefinition<TKey>> createdDefinitions, ProcessDefinition<TKey> definition)
        {
            createdDefinitions.Remove(definition);
        }

        private ICollection<ProcessDefinition<TKey>> GetDefinitions()
        {
            var builder = new ProcessDefinitionBuilder<TKey, TProcessEnum>();

            builder.Autogenerate();

            return builder.Definitions;
        }

        #endregion

    }
}
