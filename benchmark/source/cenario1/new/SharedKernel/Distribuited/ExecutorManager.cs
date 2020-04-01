using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace SharedKernel.Distribuited
{
    public class ExecutorManager
    {
        private readonly IDependencyResolver _resolver;
        private HashSet<string> _workers;

        public ExecutorManager(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }
        
        public string[] CreateWorkers(params WorkerParam[] types)
        {
            foreach (var handler in types)
            {
                var server = new Task(CreateWorker, handler,
                    CancellationToken.None, TaskCreationOptions.LongRunning);
                
                server.Start();
                _workers.Add(handler.Name);
            }

            return _workers.ToArray();
        }
        
        private void CreateWorker(object worker)
        {
            var workerParam = worker as WorkerParam;
                
            _workers.Add(workerParam.Name);
                
            using (var workerClient = new ResponseSocket($"inproc://{workerParam.Name}"))
            {
                var message = workerClient.ReceiveMultipartMessage();
                var handler = _resolver.Resolve(workerParam.Type) as IHandler;

                var response = handler.Initialize(message[2].ConvertToString());
                        
                var messageToRouter = new NetMQMessage();
                messageToRouter.Append(message[0]);
                messageToRouter.AppendEmptyFrame();
                messageToRouter.Append(JsonSerializer.Serialize(response));

                workerClient.SendMultipartMessage(messageToRouter);
            }
        }
            
        public class WorkerParam
        {
            public string Name { get; protected set; }
            public Type Type { get; protected set; }

            public WorkerParam(string name, Type type)
            {
                Name = name;
                Type = type;
            }
        }
    }
}