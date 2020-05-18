using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public interface IExecutionService<TKey, TProcessEnum>
            where TKey : IComparable
            where TProcessEnum : struct, IConvertible
    {

        TResult Execute<TResult>(TProcessEnum processName, Func<TResult> process);

        TResult Execute<TResult>(TProcessEnum processName, Func<ExecutionContext<TKey>, TResult> process);

        Task<TResult> ExecuteAsync<TResult>(TProcessEnum processName, Func<TResult> process);

        Task<TResult> ExecuteAsync<TResult>(TProcessEnum processName, Func<ExecutionContext<TKey>, TResult> process);

    }
}
