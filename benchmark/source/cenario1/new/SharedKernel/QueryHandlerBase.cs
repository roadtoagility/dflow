using System;
using System.Linq;
using NetMQ;
using NetMQ.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedKernel
{
    public abstract class QueryHandlerBase<TQuery, TResult> : IQueryHandler
    {
        public virtual void Start()
        {
            
            var task3 = new Task(() => StartThread(),
                TaskCreationOptions.LongRunning);
            task3.Start();
        }

        private void StartThread()
        {
            using (var server = new RouterSocket())
            {
                server.Bind($"inproc://{GetName()}");

                try
                {
                    while (true)
                    {
                        var message = server.ReceiveMultipartMessage();

                        var json = message[2].ConvertToString();
                        var query = JsonSerializer.Deserialize<TQuery>(json);
                        var result = Handler(query);
                        
                        var messageToRouter = new NetMQMessage();
                        messageToRouter.Append(message[0]);
                        messageToRouter.AppendEmptyFrame();
                        messageToRouter.Append(JsonSerializer.Serialize(result));

                        server.SendMultipartMessage(messageToRouter);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        public abstract string GetName();
        public abstract TResult Handler(TQuery query);
    }
}