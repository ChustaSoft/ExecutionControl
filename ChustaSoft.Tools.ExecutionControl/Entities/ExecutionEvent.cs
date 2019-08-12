using ChustaSoft.Tools.ExecutionControl.Enums;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Entities
{
    public class ExecutionEvent<TKey>
    {

        public TKey Id { get; set; }

        public TKey ExecutionId { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public ExecutionStatus Status { get; set; }


        public Execution<TKey> Execution  { get; set; }

    }
}
