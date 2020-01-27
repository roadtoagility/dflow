using System;
using System.Text.Json;
using NetMQ;
using NetMQ.Sockets;

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

            CheckActivation<TQuery, TResult>(service);
            return Send<TResult, TQuery>(query, service.GetName());
        }

        private TResult Send<TResult, TQuery>(TQuery query, string serviceName)
        {
            using (var dealer = new DealerSocket())
            {
                dealer.Connect($"inproc://{serviceName}");
                var message = JsonSerializer.Serialize(query);
                
                
                var messageToServer = new NetMQMessage();
                messageToServer.AppendEmptyFrame();
                messageToServer.Append(message);
                dealer.SendMultipartMessage(messageToServer);
                
                var json = dealer.ReceiveMultipartMessage();
                var result = JsonSerializer.Deserialize<TResult>(json[1].ConvertToString());
                return result;
            }
        }

        private void CheckActivation<TQuery, TResult>(QueryHandlerBase<TQuery, TResult> service)
        {
            if(!QueryHandlersActivation.Instance.IsActivate(service.GetName()))
            {
                service.Start();
                QueryHandlersActivation.Instance.Activate(service.GetName());
            }
        }

    }
}