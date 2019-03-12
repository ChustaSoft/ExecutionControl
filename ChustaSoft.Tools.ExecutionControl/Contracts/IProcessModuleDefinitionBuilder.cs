namespace ChustaSoft.Tools.ExecutionControl.Contracts
{
    public interface IProcessModuleDefinitionBuilder<TKey>
    {

        IProcessModuleDefinitionBuilder<TKey> New(string name, string description);

        IProcessModuleDefinitionBuilder<TKey> IsConcurrent(bool concurrent);

        IProcessDefinitionBuilder<TKey> Build();

    }
}
