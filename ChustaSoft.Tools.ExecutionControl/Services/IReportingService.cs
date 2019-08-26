using ChustaSoft.Tools.ExecutionControl.Model;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public interface IReportingService<TKey> where TKey : IComparable
    {

        ProcessExecutionSummary<TKey> Last<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible;

    }
}
