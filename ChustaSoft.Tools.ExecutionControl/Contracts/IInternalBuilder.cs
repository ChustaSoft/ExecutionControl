namespace ChustaSoft.Tools.ExecutionControl.Contracts
{
    public interface IInternalBuilder<TMainBuilder>
    {

        TMainBuilder BuildInto();

    }
}
