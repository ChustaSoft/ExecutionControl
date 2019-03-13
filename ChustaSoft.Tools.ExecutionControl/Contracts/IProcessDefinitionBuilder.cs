using ChustaSoft.Tools.ExecutionControl.Enums;
using System;


namespace ChustaSoft.Tools.ExecutionControl.Contracts
{
    public interface IProcessDefinitionBuilder<TKey>
    {

        IProcessDefinitionBuilder<TKey> New(string name, string description);

        IProcessDefinitionBuilder<TKey> New<TEnum>(TEnum enumType, string description) where TEnum : struct, IConvertible;

        IProcessDefinitionBuilder<TKey> SetType(ExecutionType type);

        IProcessDefinitionBuilder<TKey> AddModule(string name, string description, bool concurrent = false);
        
    }
}
