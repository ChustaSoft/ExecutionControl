using ChustaSoft.Tools.ExecutionControl.Enums;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ExecutionEventArgs<TKey> where TKey : IComparable
    {
        public TKey ExecutionId { get; set; }
        public ExecutionStatus Status { get; set; }
        public string Message { get; set; }
    }
}
