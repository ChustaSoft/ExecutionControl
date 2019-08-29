using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public interface IReportingService<TKey> where TKey : IComparable
    {

        IEnumerable<ProcessExecutionSummary<TKey>> Daily(DateTime day);

        IEnumerable<ProcessExecutionSummary<TKey>> Daily<TProcessEnum>(TProcessEnum process, DateTime day) where TProcessEnum : struct, IConvertible;

        ProcessExecutionSummary<TKey> Last<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible;

    }
}
