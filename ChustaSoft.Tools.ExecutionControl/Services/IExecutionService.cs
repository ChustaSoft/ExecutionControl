using System;

namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public interface IExecutionService<TKey, TProcessEnum> where TProcessEnum : struct, IConvertible
    {
    }
}
