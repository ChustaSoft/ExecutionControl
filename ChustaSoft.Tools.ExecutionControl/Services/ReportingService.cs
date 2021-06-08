using ChustaSoft.Tools.ExecutionControl.Domain;
using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public class ReportingService<TKey> : IReportingService<TKey> where TKey : IComparable
    {

        #region Fields

        private readonly IProcessExecutionSummaryBusiness<TKey> _processExecutionSummaryBusiness;

        #endregion


        #region Constructor

        public ReportingService(IProcessExecutionSummaryBusiness<TKey> processExecutionSummaryBusiness)
        {
            _processExecutionSummaryBusiness = processExecutionSummaryBusiness;
        }

        #endregion


        #region Public methods

        public ProcessExecutionSummary<TKey> Last<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible
        {
            var lastExecutionSummary = _processExecutionSummaryBusiness.Last(process);

            return lastExecutionSummary;
        }

        public IEnumerable<ProcessExecutionSummary<TKey>> Daily(DateTime day)
        {
            var executionsSummary = _processExecutionSummaryBusiness.Daily(day);

            return executionsSummary;
        }

        public IEnumerable<ProcessExecutionSummary<TKey>> Daily<TProcessEnum>(TProcessEnum process, DateTime day) where TProcessEnum : struct, IConvertible
        {
            var executionsSummary = _processExecutionSummaryBusiness.Daily(process, day);

            return executionsSummary;
        }

        #endregion

    }
}
