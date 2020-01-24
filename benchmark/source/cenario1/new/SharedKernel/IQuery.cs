using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedKernel
{
    public interface IQuery<TResult>
    {
      
    }
    
    public interface IQueryDispatcher
    {
        TResult Handle<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }

    public interface IQueryHandler<TQuery, TResult> : IQueryHandler
    {
        TResult Handle(TQuery query);
    }

    public interface IQueryHandler
    {
        void Start();
    }
    
}