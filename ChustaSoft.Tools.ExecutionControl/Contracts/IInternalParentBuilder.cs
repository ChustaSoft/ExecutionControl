namespace ChustaSoft.Tools.ExecutionControl.Contracts
{
    public interface IInternalParentBuilder<TBuilder, TData>
    {

        TBuilder Integrate(TData data);

    }
}
