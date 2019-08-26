using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Model;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public class ProcessExecutionSummaryBusiness<TKey> : ProcessExecutionSummary<TKey>, IProcessExecutionSummaryBusiness<TKey> 
            where TKey : IComparable
    {

        private readonly IExecutionRepository<TKey> _executionRepository;


        public ProcessExecutionSummaryBusiness(IExecutionRepository<TKey> executionRepository)
        {
            _executionRepository = executionRepository;
        }


        public ProcessExecutionSummary<TKey> Last<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible
        {
            var lastExecution = _executionRepository.GetLast(process);

            MapValues(lastExecution);
            SetComputedValues();

            return this;
        }


        private void MapValues(Execution<TKey> lastExecution)
        {
            this.Id = lastExecution.Id;
            this.Name = lastExecution.ProcessDefinition.Name;
            this.Description = lastExecution.ProcessDefinition.Description;
            this.BeginDate = lastExecution.BeginDate;
            this.EndDate = lastExecution.EndDate;
            this.Status = lastExecution.Status.ToString();

            foreach (var evt in lastExecution.ExecutionEvents)
                this.Events.Add(new EventSummary { Date = evt.Date, Info = evt.Summary, Status = evt.Status.ToString() });
        }

        private void SetComputedValues()
        {
            if (this.EndDate != null)
                this.ExecutionInterval = (this.EndDate - this.BeginDate).Value.TotalMinutes;
        }

    }
}
