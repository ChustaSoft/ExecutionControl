using ChustaSoft.Tools.ExecutionControl.Enums;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ExecutionContext<TKey> where TKey : IComparable
    {

        private TKey _executionId;


        internal event EventHandler<ExecutionEventArgs<TKey>> Checkpoint;


        internal ExecutionContext(TKey executionId)
        {
            _executionId = executionId;
        }


        public void AddCheckpoint(string message)
        {
            var executionEvent = GetArguments(ExecutionStatus.Running, message);

            Checkpoint.Invoke(this, executionEvent);
        }

        public void AddEndSummary(string message)
        {
            var executionEvent = GetArguments(ExecutionStatus.Finishing, message);

            Checkpoint.Invoke(this, executionEvent);
        }


        private ExecutionEventArgs<TKey> GetArguments(ExecutionStatus status, string message)
            => new ExecutionEventArgs<TKey> { ExecutionId = _executionId, Status = status, Message = message };

    }
}
