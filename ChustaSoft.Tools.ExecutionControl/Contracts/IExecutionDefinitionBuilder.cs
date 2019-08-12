using ChustaSoft.Tools.ExecutionControl.Helpers;
using System;


namespace ChustaSoft.Tools.ExecutionControl.Contracts
{

    public interface IExecutionDefinitionBuilder<TKey>
    {

        ExecutionDefinitionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TKey>> definitionBuilder);

    }


    public interface IExecutionDefinitionBuilder<TEnum, TKey> : IExecutionDefinitionBuilder<TKey> where TEnum : struct, IConvertible
    {

        ExecutionDefinitionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TEnum, TKey>> definitionBuilder);

    }

}
