using ChustaSoft.Tools.ExecutionControl.Helpers;
using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Collections.Generic;


namespace ChustaSoft.Tools.ExecutionControl.Contracts
{

    public abstract class ExecutionControlConfigurationBase<TKey>
    {

        internal IEnumerable<ProcessDefinition<TKey>> GetConfigurations()
        {
            var builder = new ExecutionDefinitionBuilder<TKey>();

            DefineExecutions(builder);

            return builder.Build();
        }


        public abstract void DefineExecutions(IExecutionDefinitionBuilder<TKey> builder);

    }


    public abstract class ExecutionControlConfigurationBase<TKey, TEnum> where TEnum : struct, IConvertible
    {

        internal IEnumerable<ProcessDefinition<TKey>> GetConfigurations()
        {
            var builder = new ExecutionDefinitionBuilder<TEnum, TKey>();

            DefineExecutions(builder);

            return builder.Build();
        }


        public abstract void DefineExecutions(IExecutionDefinitionBuilder<TEnum, TKey> builder);

    }


    public abstract class ExecutionControlConfigurationBase : ExecutionControlConfigurationBase<Guid> { }

}
