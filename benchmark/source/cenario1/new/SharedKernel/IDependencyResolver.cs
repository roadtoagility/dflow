namespace SharedKernel
{
    public interface IDependencyResolver
    {
        IQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>() where TQuery : IQuery<TResult>;
    }
}