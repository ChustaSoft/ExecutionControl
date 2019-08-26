using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ProcessExecutionSummary<TKey> where TKey : IComparable
    {

        public TKey Id { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public DateTime BeginDate { get; internal set; }
        public DateTime? EndDate { get; internal set; }
        public double ExecutionInterval { get; internal set; }
        public string Status { get; internal set; }
        public ICollection<EventSummary> Events { get; internal set; }



        public ProcessExecutionSummary()
        {
            Events = Enumerable.Empty<EventSummary>().ToList();
        }

    }
}
