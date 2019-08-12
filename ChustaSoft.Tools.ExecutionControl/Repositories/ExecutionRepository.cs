using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Model;
using Microsoft.EntityFrameworkCore;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public class ExecutionRepository<TKey> : IExecutionRepository<TKey>
    {

        #region Fields

        private readonly ExecutionControlContext<TKey> _context;

        #endregion


        #region Constructor

        public ExecutionRepository(ExecutionControlContext<TKey> context)
        {
            _context = context;
        }

        #endregion


        #region Public methods

        public void Save(Execution<TKey> execution)
        {
            using (_context)
            {
                _context.Executions.Add(execution);

                _context.SaveChanges();
            }
        }

        public void Update(Execution<TKey> execution)
        {
            using (_context)
            {
                _context.Executions.Add(execution);
                _context.Entry(execution).State = EntityState.Modified;

                _context.SaveChanges();
            }
        }

        #endregion

    }
}
