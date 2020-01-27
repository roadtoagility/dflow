using System;

namespace SharedKernel
{
    public interface IDependencyResolver
    {
        QueryHandlerBase<TQuery, TResult> Resolve<TQuery, TResult>() where TQuery : IQuery<TResult>;
        object Resolve(Type service);
    }
}