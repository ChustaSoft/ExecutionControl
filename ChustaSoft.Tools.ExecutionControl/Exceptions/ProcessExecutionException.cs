using System;

namespace ChustaSoft.Tools.ExecutionControl.Exceptions
{
    public class ProcessExecutionException : Exception
    {
        public ProcessExecutionException(string message) : base(message) { }

        public ProcessExecutionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
