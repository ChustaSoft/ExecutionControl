using ChustaSoft.Tools.ExecutionControl.Helpers;
using System;


namespace ChustaSoft.Tools.ExecutionControl.Contracts
{

    public interface IExecutionBuilder<TKey>
    {

        ExecutionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TKey>> definitionBuilder);

    }


    public interface IExecutionBuilder<TEnum, TKey> : IExecutionBuilder<TKey> where TEnum : struct, IConvertible
    {

        ExecutionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TEnum, TKey>> definitionBuilder);

    }

}
