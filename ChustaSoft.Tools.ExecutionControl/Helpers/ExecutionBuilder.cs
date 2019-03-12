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

        #region Fields

        private ProcessDefinitionBuilder<TKey> _processDefinitionBuilder;

        #endregion


        #region Properties

        public ICollection<ErrorMessage> Errors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion


        #region Constructor

        public ExecutionBuilder()
        {
            _processDefinitionBuilder = new ProcessDefinitionBuilder<TKey>();
        }

        #endregion


        #region Public methods

        public ExecutionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TKey>> definitionBuilder)
        {
            definitionBuilder.Invoke(_processDefinitionBuilder);

            return this;
        }

        public IEnumerable<ProcessDefinition<TKey>> Build() => _processDefinitionBuilder.Build();

        #endregion

    }
}
