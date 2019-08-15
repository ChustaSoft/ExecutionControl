using ChustaSoft.Tools.ExecutionControl.Configuration;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public class ExecutionBusiness<TKey> : IExecutionBusiness<TKey> where TKey : IComparable
    {

        private readonly ExecutionControlConfiguration _configuration;

        private readonly IProcessDefinitionRepository<TKey> _processDefinitionRepository;
        private readonly IExecutionRepository<TKey> _executionRepository;
        

        public ExecutionBusiness(ExecutionControlConfiguration configuration, IProcessDefinitionRepository<TKey> processDefinitionRepository, IExecutionRepository<TKey> executionRepository) : base()
        {
            _configuration = configuration;

            _processDefinitionRepository = processDefinitionRepository;
            _executionRepository = executionRepository;
        }


        public Execution<TKey> Register(string processName)
        {
            var definition = _processDefinitionRepository.Get(processName);
            var execution = new Execution<TKey>()
            {
                BeginDate = DateTime.UtcNow,
                Host = Environment.MachineName,
                ProcessDefinitionId = definition.Id,
                Result = ExecutionResult.Unknown,
                Status = ExecutionStatus.Waiting
            };

            _executionRepository.Save(execution);

            return execution;
        }

        public TKey Abort(TKey processDefinitionId)
        {
            var previousExecution = _executionRepository.GetLastBlocked(processDefinitionId);

            PerformUpdate(previousExecution, ExecutionStatus.Aborted, ExecutionResult.Error);

            return previousExecution.Id;
        }

        public TKey Block(Execution<TKey> execution)
        {
            PerformUpdate(execution, ExecutionStatus.Blocked, ExecutionResult.Warning);

            _executionRepository.Update(execution);

            return execution.Id;
        }

        public TKey Complete(Execution<TKey> execution, ExecutionResult result)
        {
            PerformUpdate(execution, ExecutionStatus.Finished, result);

            _executionRepository.Update(execution);

            return execution.Id;
        }

        public ExecutionAvailability IsAllowed(Execution<TKey> execution)
        {
            var lastExecution = _executionRepository.GetLastCompleted(execution.ProcessDefinitionId);

            if (lastExecution == null || lastExecution?.Status == ExecutionStatus.Finished || lastExecution?.Status == ExecutionStatus.Aborted)
                return ExecutionAvailability.Available;
            else if (ProcessMustBeAborted(lastExecution))
                return ExecutionAvailability.Abort;
            else
                return ExecutionAvailability.Block;
        }


        private bool ProcessMustBeAborted(Execution<TKey> lastExecution)
            => lastExecution?.BeginDate < DateTime.UtcNow.AddMinutes(-1 * _configuration.MinutesToAbort);

        private void PerformUpdate(Execution<TKey> previousExecution, ExecutionStatus status, ExecutionResult result)
        {
            previousExecution.EndDate = DateTime.UtcNow;
            previousExecution.Status = status;
            previousExecution.Result = result;

            _executionRepository.Update(previousExecution);
        }

    }
}
