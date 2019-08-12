using System;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Repositories;

namespace ChustaSoft.Tools.ExecutionControl.Model
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

        public void Abort(TKey processDefinitionId)
        {
            var previousExecution = _executionRepository.GetLastBlocked(processDefinitionId);

            previousExecution.EndDate = DateTime.UtcNow;
            previousExecution.Status = ExecutionStatus.Aborted;
            previousExecution.Result = ExecutionResult.Error;

            _executionRepository.Update(previousExecution);
        }

        public void Block(Execution<TKey> execution)
        {
            execution.EndDate = DateTime.UtcNow;
            execution.Status = ExecutionStatus.Blocked;
            execution.Result = ExecutionResult.Warning;

            _executionRepository.Update(execution);
        }

        public void Complete(Execution<TKey> execution, ExecutionResult result)
        {
            execution.EndDate = DateTime.UtcNow;
            execution.Result = result;
            execution.Status = ExecutionStatus.Finished;
        }

        public ExecutionAvailability IsAllowed(Execution<TKey> execution)
        {
            var lastExecution = _executionRepository.GetLastCompleted(execution.ProcessDefinitionId);

            if (lastExecution?.Status == ExecutionStatus.Finished || lastExecution?.Status == ExecutionStatus.Aborted)
                return ExecutionAvailability.Available;
            else if (ProcessMustBeAborted(lastExecution))
                return ExecutionAvailability.Abort;
            else
                return ExecutionAvailability.Block;
        }


        private bool ProcessMustBeAborted(Execution<TKey> lastExecution)
            => lastExecution?.BeginDate < DateTime.UtcNow.AddMinutes(-1 * _configuration.MinutesToAbort);
        
    }
}
