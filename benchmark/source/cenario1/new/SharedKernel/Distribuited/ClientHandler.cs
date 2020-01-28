using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace SharedKernel.Distribuited
{
    public sealed class ClientHandler : IOutputTransport
    {
        private readonly string _serverEndpoint;

        public ClientHandler(string serverEndpoint)
        {
            _serverEndpoint = serverEndpoint;
        }
        
        public TResult Handle<TResult, TQuery>(TQuery query)
        {
            return JsonSerializer.Deserialize<TResult>(SendRequest(JsonSerializer.Serialize(query)));
        }

        private string SendRequest(string request)
        {
            using (var server = new DealerSocket())
            {
                server.Options.Identity = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

                var ok = false;
                do
                {
                    try
                    {
                        server.Connect($"{_serverEndpoint}");
                        ok = true;

                    }
                    catch (NetMQException ex)
                    {
                             
                    }
                         
                } while (!ok);
                     
                     
                var msg = new NetMQMessage();
                msg.AppendEmptyFrame();
                msg.Append(request);

                server.SendMultipartMessage(msg);
                     
                var result = server.ReceiveMultipartMessage();

                return result[1].ConvertToString();
            }
        }
    }
}