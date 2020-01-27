namespace SharedKernel.Distribuited
{
    public interface IOutputTransport
    {
        TResult Send<TResult, TQuery>(TQuery query);
    }
}