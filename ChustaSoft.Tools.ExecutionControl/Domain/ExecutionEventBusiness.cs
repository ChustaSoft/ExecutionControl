using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public class ExecutionEventBusiness<TKey> : IExecutionEventBusiness<TKey> where TKey : IComparable
    {

        #region Fields

        private readonly IExecutionEventRepository<TKey> _executionEventRepository;

        #endregion


        #region Constructor

        public ExecutionEventBusiness(IExecutionEventRepository<TKey> executionEventRepository)
        {
            _executionEventRepository = executionEventRepository;
        }

        #endregion


        #region Public methods

        public bool Create(TKey executionId, ExecutionStatus status, string message)
        {
            var executionEvent = GenerateExecutionEvent(executionId, status, message);

            return _executionEventRepository.Create(executionEvent);
        }

        public bool Create(ExecutionEventArgs<TKey> executionEventArgs)
        {
            var executionEvent = GenerateExecutionEvent(executionEventArgs.ExecutionId, executionEventArgs.Status, executionEventArgs.Message);

            return _executionEventRepository.Create(executionEvent);
        }

        #endregion


        #region Private methods

        private static ExecutionEvent<TKey> GenerateExecutionEvent(TKey executionId, ExecutionStatus status, string message)
            => new ExecutionEvent<TKey>()
            {
                ExecutionId = executionId,
                Date = DateTime.UtcNow,
                Status = status,
                Summary = message
            };

        #endregion

    }
}
