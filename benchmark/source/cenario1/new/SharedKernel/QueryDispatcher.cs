using System;

namespace SharedKernel
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IDependencyResolver _resolver;

        public QueryDispatcher(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public TResult Handle<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var service = _resolver.Resolve<TQuery, TResult>();
            return service.Handle(query);
        }
    }
}