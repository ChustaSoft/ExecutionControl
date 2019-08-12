using ChustaSoft.Tools.ExecutionControl.Entities;
using Newtonsoft.Json;

namespace ChustaSoft.Tools.ExecutionControl.Model
{

    public class ExecutionContext<TKey, TContext> : Execution<TKey> where TContext : class
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
