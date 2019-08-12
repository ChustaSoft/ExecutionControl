using ChustaSoft.Common.Helpers;
using ChustaSoft.Common.Utilities;
using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public class ExecutionService<TKey, TProcessEnum> : IExecutionService where TProcessEnum : struct, IConvertible
    {

        private readonly ExecutionControlConfiguration _configuration;

        private readonly IDictionary<TProcessEnum, ProcessContextDefinition> _definitions;
        private readonly ExecutionControlContext<TKey> _dbContext;


        public ExecutionService(ExecutionControlConfiguration configuration, ExecutionControlContext<TKey> dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }


        public void Execute(TProcessEnum process)
        {
            //Get previous data
            //Invoke method
        }


        private bool IsAllowed(TProcessEnum process)
        {
            var tmpLastRun = GetLastRunStatus(process);
            var definition = _definitions[process];

            if (tmpLastRun?.Status == ExecutionStatus.Finished || tmpLastRun?.Status == ExecutionStatus.Aborted)
                return PerformSuccessfulStart(definition);
            else if (tmpLastRun?.BeginDate < DateTime.UtcNow.AddMinutes(-1 * _configuration.MinutesToAbort))
                return PerformAbortPreviousExecution(process, tmpLastRun);
            else if (tmpLastRun?.Status == ExecutionStatus.Running)
                return PerformBlockExecution(definition, "Aborted because the last job is still running");
            else
                return PerformBlockExecution(definition, "Process has been blocked");
        }

        private bool PerformBlockExecution(ProcessContextDefinition definition, string statusSummary)
        {
            RegisterStartAttempt(definition, ExecutionStatus.Blocked, statusSummary);
            return false;
        }

        private bool PerformAbortPreviousExecution(TProcessEnum process, Execution<TKey> tmpLastRun)
        {
            AbortPreviousProcess(tmpLastRun);
            return IsAllowed(process);
        }

        private bool PerformSuccessfulStart(ProcessContextDefinition definition)
        {
            RegisterStartAttempt(definition, ExecutionStatus.Running, "Process started");
            return true;
        }

        private void AbortPreviousProcess(Execution<TKey> tmpLastRun)
        {
            var abortStatus = ExecutionStatus.Aborted;

            tmpLastRun.Status = abortStatus;
            tmpLastRun.EndDate = DateTime.UtcNow;
            tmpLastRun._EndContext = tmpLastRun._EndContext;

            RegisterEvent(abortStatus, $"Aborted previous process due to > {_configuration.MinutesToAbort} minutes running", tmpLastRun.Id);
        }

        private Execution<TKey> GetLastRunStatus(TProcessEnum process)
        {
            return _dbContext.Executions
                .Include(SelectablePropertiesBuilder<Execution<TKey>>.GetProperties().SelectProperty(x => x.ProcessDefinition).FormatSelection())
                .Where(x => x.ProcessDefinition.Name == process.ToString())
                .OrderByDescending(x => x.BeginDate)
                .FirstOrDefault();
        }

        private void RegisterStartAttempt(ProcessContextDefinition lastExecution, ExecutionStatus startStatus, string statusSummary)
        {
            var newExecution = new Execution<TKey>
            {
                BeginDate = DateTime.UtcNow,
                Host = Environment.MachineName,
                ProcessDefinitionId = lastExecution.Id,
                Status = startStatus
            };

            RegisterExecution(lastExecution, newExecution);
            RegisterEvent(startStatus, statusSummary, newExecution.Id);
        }

        private void RegisterEvent(ExecutionStatus status, string statusSummary, TKey executionId)
        {
            _dbContext.ExecutionEvents.Add(new ExecutionEvent<TKey>
            {
                ExecutionId = executionId,
                Status = status,
                Date = DateTime.UtcNow,
                Summary = statusSummary
            });
            _dbContext.SaveChanges();
        }

        private void RegisterExecution(ProcessContextDefinition lastExecution, Execution<TKey> newExecution)
        {
            _dbContext.Executions.Add(newExecution);
            _dbContext.SaveChanges();
        }
    }
}
