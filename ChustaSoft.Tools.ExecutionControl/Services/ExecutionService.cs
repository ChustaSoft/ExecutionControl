using ChustaSoft.Tools.ExecutionControl.Model;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using System;
using System.Collections.Generic;


namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public class ExecutionService<TKey> : IExecutionService<TKey>
    {

        #region Fields

        private readonly IProcessDefinitionRepository<TKey> _processDefinitionRepository;

        #endregion


        #region Constructor

        public ExecutionService(IProcessDefinitionRepository<TKey> processDefinitionRepository)
        {
            _processDefinitionRepository = processDefinitionRepository;
        }

        #endregion


        #region Public methods

        public bool SaveDefinition(ProcessDefinition<TKey> definition)
        {
            return _processDefinitionRepository.Save(definition);
        }

        public bool SaveDefinitions(IEnumerable<ProcessDefinition<TKey>> definitions)
        {
            return _processDefinitionRepository.Save(definitions);
        }

        public void TryExecute<T>(Func<T> process)
        {


            /**
             * 1. Register attempt of execution
             * 2. Get last execution for process? 
             * 3. Execute if possible
             * 4. Mark execution as finished
             */

            throw new NotImplementedException();
        }

        #endregion

    }
}
