using System;

namespace SharedKernel
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TResult Handle<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var service = this._serviceProvider.GetService(typeof(IQueryHandler<TQuery,TResult>)) as IQueryHandler<TQuery,TResult>; 
            return service.Handle(query);
        }
    }
}