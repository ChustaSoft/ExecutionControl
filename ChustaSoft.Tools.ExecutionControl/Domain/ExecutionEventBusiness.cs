using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public class ExecutionEventBusiness<TKey> : IExecutionEventBusiness<TKey> where TKey : IComparable
    {

        private readonly IExecutionEventRepository<TKey> _executionEventRepository;


        public ExecutionEventBusiness(IExecutionEventRepository<TKey> executionEventRepository)
        {
            _executionEventRepository = executionEventRepository;
        }


        public bool Create(TKey executionId, ExecutionStatus status, string message)
        {
            var executionEvent = new ExecutionEvent<TKey>()
            {
                ExecutionId = executionId,
                Date = DateTime.UtcNow,
                Status = status,
                Summary = message
            };

            return _executionEventRepository.Create(executionEvent);
        }

    }
}
