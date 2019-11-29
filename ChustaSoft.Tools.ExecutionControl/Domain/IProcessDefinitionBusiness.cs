using ChustaSoft.Tools.ExecutionControl.Entities;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Domain
{
    public interface IProcessDefinitionBusiness<TKey, TProcessEnum>
            where TKey : IComparable
            where TProcessEnum : struct, IConvertible
    {

        ProcessDefinition<TKey> Get(TProcessEnum processEnum);

        void Setup();

    }
}
