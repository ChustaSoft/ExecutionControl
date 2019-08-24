using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public interface IProcessDefinitionBusiness<TKey, TProcessEnum>
            where TKey : IComparable
            where TProcessEnum : struct, IConvertible
    {

        void Setup();

    }
}
