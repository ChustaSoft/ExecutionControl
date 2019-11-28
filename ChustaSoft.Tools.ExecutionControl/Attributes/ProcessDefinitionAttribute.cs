using ChustaSoft.Tools.ExecutionControl.Enums;
using System;
using System.ComponentModel;

namespace ChustaSoft.Tools.ExecutionControl.Attributes
{

    [AttributeUsage(AttributeTargets.Field)]
    public class ProcessDefinitionAttribute : DescriptionAttribute
    {

        public ProcessType Type { get; private set; }


        public ProcessDefinitionAttribute(ProcessType type, string description)
            : base(description)
        {
            Type = type;
        }

    }
}
