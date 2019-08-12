using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public interface IExecutionBusiness<TKey> where TKey : IComparable
    {

        Execution<TKey> Register(string processName);

        void Abort(TKey processDefinitionId);

        void Block(Execution<TKey> execution);

        ExecutionAvailability IsAllowed(Execution<TKey> execution);

    }
}