namespace SharedKernel.Distribuited
{
    public interface IOutputTransport
    {
        TResult Handle<TResult, TQuery>(TQuery query);
    }
}