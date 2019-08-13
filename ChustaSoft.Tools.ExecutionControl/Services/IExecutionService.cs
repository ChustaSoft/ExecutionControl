using System;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public interface IExecutionService<TKey, TProcessEnum>
        where TKey : IComparable
        where TProcessEnum : struct, IConvertible
    {

        void Execute<T>(TProcessEnum processName, Func<T> process);

    }
}
