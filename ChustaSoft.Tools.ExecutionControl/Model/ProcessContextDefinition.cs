using ChustaSoft.Tools.ExecutionControl.Entities;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ProcessContextDefinition
    {

        public Action Method { get; set; }

        private object _context;

        internal T Context<T>() => (T)_context;
        internal void Context<T>(T context)
        {
            _context = context;
        }

    }
}
