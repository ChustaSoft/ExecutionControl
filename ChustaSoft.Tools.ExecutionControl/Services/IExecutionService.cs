using ChustaSoft.Tools.ExecutionControl.Model;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public interface IExecutionService<TKey, TProcessEnum>
            where TKey : IComparable
            where TProcessEnum : struct, IConvertible
    {

        TResult Execute<TResult>(TProcessEnum processName, Func<TResult> process);

        TResult Execute<TResult>(TProcessEnum processName, Func<ExecutionContext<TKey>, TResult> process);

    }
}
