using ChustaSoft.Common.Helpers;
using ChustaSoft.Common.Utilities;
using ChustaSoft.Tools.ExecutionControl.Contracts;
using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Collections.Generic;


namespace ChustaSoft.Tools.ExecutionControl.Helpers
{
    public class ExecutionBuilder<TKey> : IExecutionBuilder<TKey>, IBuilder<IEnumerable<ProcessDefinition<TKey>>>
    {

        private ProcessDefinitionBuilder<TKey> _processDefinitionBuilder;


        public ExecutionBuilder()
        {
            _processDefinitionBuilder = new ProcessDefinitionBuilder<TKey>();
        }


        public ExecutionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TKey>> definitionBuilder)
        {
            definitionBuilder.Invoke(_processDefinitionBuilder);

            return this;
        }

        public ICollection<ErrorMessage> Errors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<ProcessDefinition<TKey>> Build() => _processDefinitionBuilder.BuildAll();

    }
}
