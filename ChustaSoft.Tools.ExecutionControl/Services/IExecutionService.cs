using ChustaSoft.Tools.ExecutionControl.Model;
using System;
using System.Collections.Generic;


namespace ChustaSoft.Tools.ExecutionControl.Services
{
    public interface IExecutionService<TKey>
    {

        bool SaveDefinition(ProcessDefinition<TKey> definition);

        bool SaveDefinitions(IEnumerable<ProcessDefinition<TKey>> definitions);

        void TryExecute<T>(Func<T> process);

    }
}
