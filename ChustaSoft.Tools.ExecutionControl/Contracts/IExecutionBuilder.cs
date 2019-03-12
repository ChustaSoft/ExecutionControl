using ChustaSoft.Tools.ExecutionControl.Helpers;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Contracts
{
    public interface IExecutionBuilder<TKey>
    {

        ExecutionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TKey>> definitionBuilder);

    }
}
