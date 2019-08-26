using System;
using ChustaSoft.Tools.ExecutionControl.Domain;
using ChustaSoft.Tools.ExecutionControl.Model;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public class ReportingService<TKey> : IReportingService<TKey> where TKey : IComparable
    {

        private readonly IProcessExecutionSummaryBusiness<TKey> _processExecutionSummaryBusiness;


        public ReportingService(IProcessExecutionSummaryBusiness<TKey> processExecutionSummaryBusiness)
        {
            _processExecutionSummaryBusiness = processExecutionSummaryBusiness;
        }


        public ProcessExecutionSummary<TKey> Last<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible
        {
            var lastExecutionSummary = _processExecutionSummaryBusiness.Last(process);

            return lastExecutionSummary;
        }

    }
}
