using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Model;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public class ProcessExecutionSummaryBusiness<TKey> : IProcessExecutionSummaryBusiness<TKey>
            where TKey : IComparable
    {

        #region Fields

        private readonly IExecutionRepository<TKey> _executionRepository;

        #endregion


        #region Constructor

        public ProcessExecutionSummaryBusiness(IExecutionRepository<TKey> executionRepository)
        {
            _executionRepository = executionRepository;
        }

        #endregion


        #region Public methods

        public IEnumerable<ProcessExecutionSummary<TKey>> Daily(DateTime day)
        {
            var lastExecutions = _executionRepository.GetDaily(day);

            foreach (var execution in lastExecutions)
                yield return CreateModel(execution);
        }

        public IEnumerable<ProcessExecutionSummary<TKey>> Daily<TProcessEnum>(TProcessEnum process, DateTime day) where TProcessEnum : struct, IConvertible
        {
            var lastExecutions = _executionRepository.GetDaily(process, day);

            foreach (var execution in lastExecutions)
                yield return CreateModel(execution);
        }

        public ProcessExecutionSummary<TKey> Last<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible
        {
            var lastExecution = _executionRepository.GetLast(process);
            var summary = CreateModel(lastExecution);

            return summary;
        }

        #endregion


        #region Private methods

        private ProcessExecutionSummary<TKey> CreateModel(Execution<TKey> lastExecution)
        {
            var summary = new ProcessExecutionSummary<TKey>();

            MapValues(summary, lastExecution);
            SetComputedValues(summary);

            return summary;
        }

        private void MapValues(ProcessExecutionSummary<TKey> summary, Execution<TKey> lastExecution)
        {
            summary.Id = lastExecution.Id;
            summary.Name = lastExecution.ProcessDefinition.Name;
            summary.Description = lastExecution.ProcessDefinition.Description;
            summary.BeginDate = lastExecution.BeginDate;
            summary.EndDate = lastExecution.EndDate;
            summary.Status = lastExecution.Status.ToString();

            foreach (var evt in lastExecution.ExecutionEvents)
                summary.Events.Add(new EventSummary { Date = evt.Date, Info = evt.Summary, Status = evt.Status.ToString() });
        }

        private void SetComputedValues(ProcessExecutionSummary<TKey> summary)
        {
            if (summary.EndDate != null)
                summary.ExecutionInterval = (summary.EndDate - summary.BeginDate).Value.TotalMinutes;
        }

        #endregion

    }
}
