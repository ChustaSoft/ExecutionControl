using ChustaSoft.Tools.ExecutionControl.Configuration;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public class ExecutionBusiness<TKey> : IExecutionBusiness<TKey> where TKey : IComparable
    {

        #region Fields

        private readonly ExecutionControlConfiguration _configuration;

        private readonly IProcessDefinitionRepository<TKey> _processDefinitionRepository;
        private readonly IExecutionRepository<TKey> _executionRepository;

        #endregion


        #region Constructor

        public ExecutionBusiness(ExecutionControlConfiguration configuration, IProcessDefinitionRepository<TKey> processDefinitionRepository, IExecutionRepository<TKey> executionRepository) : base()
        {
            _configuration = configuration;

            _processDefinitionRepository = processDefinitionRepository;
            _executionRepository = executionRepository;
        }

        #endregion


        #region Public methods

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

        public TKey Start(Execution<TKey> execution)
        {
            execution.Status = ExecutionStatus.Running;

            _executionRepository.Update(execution);

            return execution.Id;
        }

        public TKey Abort(Execution<TKey> execution)
        {
            var previousExecution = _executionRepository.GetLastDead(execution);

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
            var lastExecution = _executionRepository.GetLastCompleted(execution);

            if (lastExecution == null || lastExecution?.Status == ExecutionStatus.Finished || lastExecution?.Status == ExecutionStatus.Aborted)
                return ExecutionAvailability.Available;
            else if (ProcessMustBeAborted(lastExecution))
                return ExecutionAvailability.Abort;
            else
                return ExecutionAvailability.Block;
        }

        #endregion


        #region Private methods

        private void PerformUpdate(Execution<TKey> previousExecution, ExecutionStatus status, ExecutionResult result)
        {
            previousExecution.EndDate = DateTime.UtcNow;
            previousExecution.Status = status;
            previousExecution.Result = result;

            _executionRepository.Update(previousExecution);
        }

        private bool ProcessMustBeAborted(Execution<TKey> lastExecution)
            => lastExecution?.BeginDate < DateTime.UtcNow.AddMinutes(-1 * _configuration.MinutesToAbort);

        #endregion

    }
}
