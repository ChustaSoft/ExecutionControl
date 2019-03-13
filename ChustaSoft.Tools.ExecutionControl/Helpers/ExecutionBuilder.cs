using ChustaSoft.Common.Enums;
using ChustaSoft.Common.Helpers;
using ChustaSoft.Common.Utilities;
using ChustaSoft.Tools.ExecutionControl.Contracts;
using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ChustaSoft.Tools.ExecutionControl.Helpers
{
    public class ExecutionBuilder<TKey> : IExecutionBuilder<TKey>, IBuilder<IEnumerable<ProcessDefinition<TKey>>>
    {

        #region Fields

        protected ICollection<ErrorMessage> _errors;

        private ProcessDefinitionBuilder<TKey> _processDefinitionBuilder;

        #endregion


        #region Properties

        public ICollection<ErrorMessage> Errors
        {
            get { Build(); return _errors; }
            set => _errors = value;
        }

        #endregion


        #region Constructor

        public ExecutionBuilder()
        {
            _errors = new List<ErrorMessage>();
            _processDefinitionBuilder = new ProcessDefinitionBuilder<TKey>();
        }

        #endregion


        #region Public methods

        public ExecutionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TKey>> definitionBuilder)
        {
            definitionBuilder.Invoke(_processDefinitionBuilder);

            return this;
        }

        public IEnumerable<ProcessDefinition<TKey>> Build()
        {
            var definitions = _processDefinitionBuilder.Build();

            return definitions;
        }

        #endregion

    }


    public class ExecutionBuilder<TEnum, TKey> : ExecutionBuilder<TKey>, IExecutionBuilder<TEnum, TKey> where TEnum : struct, IConvertible
    {

        #region Fields

        private ProcessDefinitionBuilder<TEnum, TKey> _processDefinitionBuilder;

        #endregion


        #region Properties

        public new ICollection<ErrorMessage> Errors
        {
            get { Build(); return _errors; }
            set => _errors = value;
        }

        #endregion


        #region Constructor

        public ExecutionBuilder()
        {
            _processDefinitionBuilder = new ProcessDefinitionBuilder<TEnum, TKey>();
        }

        #endregion


        #region Public methods

        public ExecutionBuilder<TKey> Generate(Action<ProcessDefinitionBuilder<TEnum, TKey>> definitionBuilder)
        {
            definitionBuilder.Invoke(_processDefinitionBuilder);

            return this;
        }

        public new IEnumerable<ProcessDefinition<TKey>> Build()
        {
            var definitions = _processDefinitionBuilder.Build();

            CheckDefinitions(definitions);

            return definitions;
        }

        #endregion


        #region Private methods

        private void CheckDefinitions(IEnumerable<ProcessDefinition<TKey>> definitions)
        {
            foreach (var element in EnumsHelper.GetEnumList<TEnum>())
                if (!definitions.Any(x => x.Name == element.ToString()))
                {
                    var errorMessage = $"Definition missing during configuration: {element.ToString()}. All Enum members must have is own configuration\n";
                    
                    _errors.Add(new ErrorMessage(ErrorType.Required, errorMessage));
                }
        }

        #endregion

    }

}
