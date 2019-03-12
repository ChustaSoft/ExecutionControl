using ChustaSoft.Tools.ExecutionControl.Contracts;
using ChustaSoft.Tools.ExecutionControl.Model;


namespace ChustaSoft.Tools.ExecutionControl.Helpers
{
    public class ProcessModuleDefinitionBuilder<TKey> : IProcessModuleDefinitionBuilder<TKey>
    {

        private ProcessModuleDefinition<TKey> _processModuleDefinition;
        private IInternalParentBuilder<IProcessDefinitionBuilder<TKey>, ProcessModuleDefinition<TKey>> _parentBuilder;


        internal ProcessModuleDefinitionBuilder(ProcessDefinitionBuilder<TKey> parentBuilder)
        {
            _parentBuilder = parentBuilder;
            _processModuleDefinition = new ProcessModuleDefinition<TKey>();
        }

        public IProcessModuleDefinitionBuilder<TKey> New(string name, string description)
        {
            _processModuleDefinition.Name = name;
            _processModuleDefinition.Description = description;

            return this;
        }

        public IProcessModuleDefinitionBuilder<TKey> IsConcurrent(bool concurrent)
        {
            _processModuleDefinition.Concurrent = concurrent;

            return this;
        }

        public IProcessDefinitionBuilder<TKey> Build() => _parentBuilder.Integrate(_processModuleDefinition);
       
    }
}
