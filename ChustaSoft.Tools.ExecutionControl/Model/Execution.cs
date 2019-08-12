using ChustaSoft.Tools.ExecutionControl.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class Execution<TKey>
    {

        public TKey Id { get; set; }

        public TKey ProcessDefinitionId { get; set; }

        public ExecutionStatus Status { get; set; }

        public ExecutionResult Result { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Host { get; set; }

        internal string _BeginContext { get; set; }

        internal string _EndContext { get; set; }


        public ProcessDefinition<TKey> ProcessDefinition { get; set; }

        public IEnumerable<ExecutionEvent<TKey>> ExecutionEvents { get; set; }

        
        public Execution()
        {
            ExecutionEvents = Enumerable.Empty<ExecutionEvent<TKey>>();
        }

    }


    public class Execution<TKey, TContext> : Execution<TKey> where TContext : class
    {

        public TContext BeginContext
        {
            get { return _BeginContext == null ? null : JsonConvert.DeserializeObject<TContext>(_BeginContext); }
            set { _BeginContext = JsonConvert.SerializeObject(value); }
        }

        public TContext EndContext
        {
            get { return _EndContext == null ? null : JsonConvert.DeserializeObject<TContext>(_EndContext); }
            set { _EndContext = JsonConvert.SerializeObject(value); }
        }

    }

}
