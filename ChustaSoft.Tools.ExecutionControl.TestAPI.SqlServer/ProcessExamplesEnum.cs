using ChustaSoft.Tools.ExecutionControl.Attributes;
using ChustaSoft.Tools.ExecutionControl.Enums;
using System.ComponentModel;

namespace ChustaSoft.Tools.ExecutionControl.TestAPI.Enums
{
    public enum ProcessExamplesEnum
    {
        [Description("Execution Process 1")]
        Process1,
        [Description("Execution Process 2")]
        Process2,

        [ProcessDefinition(ProcessType.Background, "Background process")]
        BackgroundTestProcess

    }
}