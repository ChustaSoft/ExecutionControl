using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Model;
using System.Collections.Generic;


namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public class ProcessDefinitionRepository<TKey> : IProcessDefinitionRepository<TKey>
    {

        #region Fields

        private readonly ExecutionControlContext<TKey> _context;

        #endregion


        #region Constructor

        public ProcessDefinitionRepository(ExecutionControlContext<TKey> context)
        {
            _context = context;
        }

        #endregion


        #region Public methods

        public bool Save(ProcessDefinition<TKey> definition)
        {
            using (_context)
            {
                _context.ProcessDefinitions.Add(definition);

                return _context.SaveChanges() > 0;
            }
        }

        public bool Save(IEnumerable<ProcessDefinition<TKey>> definitions)
        {
            using (_context)
            {
                _context.ProcessDefinitions.AddRange(definitions);

                return _context.SaveChanges() > 0;
            }
        }

        #endregion

    }
}
