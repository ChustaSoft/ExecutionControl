using ChustaSoft.Tools.ExecutionControl.Attributes;
using ChustaSoft.Tools.ExecutionControl.Enums;
using System.ComponentModel;

namespace ChustaSoft.Tools.ExecutionControl.UnitTest
{
    public enum TestUndefinedProcesses
    {
        Process1,
        Process2,
        Process3
    }

    public enum TestDefinedProcesses
    {
        [Description("Execution Process 1")]
        Process1,
        [Description("Execution Process 2")]
        Process2,
        [Description("Execution Process 3")]
        Process3
    }

    public enum TestExtraDefinedProcesses
    {
        [ProcessDefinition(ProcessType.Background, "Execution Process 1")]
        Process1,
        [ProcessDefinition(ProcessType.Batch, "Execution Process 1")]
        Process2
    }

}
