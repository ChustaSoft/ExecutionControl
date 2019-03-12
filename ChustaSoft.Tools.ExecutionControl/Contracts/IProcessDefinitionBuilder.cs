using ChustaSoft.Tools.ExecutionControl.Enums;


namespace ChustaSoft.Tools.ExecutionControl.Contracts
{
    public interface IProcessDefinitionBuilder<TKey>
    {

        IProcessDefinitionBuilder<TKey> New(string name, string description);

        IProcessDefinitionBuilder<TKey> SetType(ExecutionType type);

        IProcessModuleDefinitionBuilder<TKey> AddModule(string name, string description);

    }
}
