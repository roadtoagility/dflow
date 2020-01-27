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


    // public interface IQueryHandler
    // {
    //     // void Start();
    //     object Handle(object query);
    // }
    
    
    public interface IQueryHandler<TQuery, TResult>
    {
        // void Start();
        TResult Handle(TQuery query);// where TQuery : IQuery<TResult>;
    }
}