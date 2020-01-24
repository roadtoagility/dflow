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

            using (var dealer = new DealerSocket())
            {
                dealer.Connect($"inproc://{service.GetName()}");
                var message = JsonSerializer.Serialize(query);
                
                
                var messageToServer = new NetMQMessage();
                messageToServer.AppendEmptyFrame();
                messageToServer.Append(message);
                dealer.SendMultipartMessage(messageToServer);
                
                var json = dealer.ReceiveMultipartMessage();
                var result = JsonSerializer.Deserialize<TResult>(json[2].ConvertToString());
                return result;
            }
        }
    }
}