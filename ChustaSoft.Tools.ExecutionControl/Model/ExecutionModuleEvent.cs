using ChustaSoft.Tools.ExecutionControl.Enums;


namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ExecutionModuleEvent<TKey>
    {

        public TKey Id { get; set; }

        public TKey ExecutionModuleId { get; set; }

        public string Summary { get; set; }

        public ExecutionStatus Status { get; set; }


        public ExecutionModule<TKey> ExecutionModule { get; set; }

    }
}
