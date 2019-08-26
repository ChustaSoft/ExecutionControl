using ChustaSoft.Tools.ExecutionControl.Model;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public interface IProcessExecutionSummaryBusiness<TKey> where TKey : IComparable
    {
        ProcessExecutionSummary<TKey> Last<TProcessEnum>(TProcessEnum process) where TProcessEnum : struct, IConvertible;
    }
}
