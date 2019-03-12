using ChustaSoft.Tools.ExecutionControl.Enums;


namespace ChustaSoft.Tools.ExecutionControl.Model
{
    public class ExecutionEvent<TKey>
    {

        public TKey Id { get; set; }

        public TKey ExecutionId { get; set; }

        public string Summary { get; set; }

        public ExecutionStatus Status { get; set; }


        public Execution<TKey> Execution { get; set; }

    }
}
