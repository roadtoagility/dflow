using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace SharedKernel.Distribuited
{
    public sealed class QueryHandler : ITransport<RouterSocket, DealerSocket>
        {
            private readonly RouterSocket _input;
            private readonly DealerSocket _output;

            private readonly String _externalEndpoint;
            private readonly String _internalEndpoint;

            /// <summary>
            /// TODO: Implementar um construtor para receber uma instância da interface IConfiguration
            /// INcluir um construtor para receber uma instancia da interface IConfiguration
            /// que terá lido o appsettings.json ou appsettings
            /// </summary>
            /// <param name="externalEndpoint"></param>
            public QueryHandler(String externalEndpoint)
            {
                _externalEndpoint = externalEndpoint;  
                
                // var server = new Task(StartExternalEndpoint,_externalEndpoint,
                //     CancellationToken.None, TaskCreationOptions.LongRunning);
                //
                // server.Start();   
            }
                
            public QueryHandler(String externalEndpoint, String internalEndpoint)
                :this(externalEndpoint)
            {
                _internalEndpoint = internalEndpoint;

                if (!String.IsNullOrEmpty(_internalEndpoint))
                {
                    _output = new DealerSocket();
                }
            }

            public void Start()
            {
                var server = new Task(StartExternalEndpoint,_externalEndpoint,
                    CancellationToken.None, TaskCreationOptions.LongRunning);
                
                server.Start();     
            }
            
            private void StartExternalEndpoint(object? endpoint)
            {
                using (var server = new RouterSocket())
                {
                    server.Options.Identity = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                    server.Options.RouterMandatory = true;
                    server.Bind($"inproc://{endpoint.ToString()}");
                    var ok = false;

                    do
                    {
                        try
                        {
                            _output.Connect($"inproc://{_internalEndpoint}");
                            ok = true;
                        }
                        catch (NetMQException ex)
                        {

                        }
                    } while (!ok);

                    server.ReceiveReady += (sender, args) =>
                    {
                        // está se conectando em mim então eu posso me conectar de volta.
                        
                        // var id = args.Socket.ReceiveFrameString();
                        // var comando = args.Socket.ReceiveFrameString();
                        // var empty = args.Socket.ReceiveFrameString();
                        // var dados = args.Socket.ReceiveFrameString();
                    };

                    try
                    {
                        while (true)
                        {
                            var message = server.ReceiveMultipartMessage();

                            //TODO: roteamento deverá ter vários worker para efetivamente processar os requests
                            
                            var messageToExecutor = new NetMQMessage();
                            
                            messageToExecutor.AppendEmptyFrame();
                            var serverMessage = message[2];
                            messageToExecutor.Append(serverMessage);
                            
                            _output.SendMultipartMessage(messageToExecutor);

                            var result = _output.ReceiveMultipartMessage();
                            var messageToRouter = new NetMQMessage();
                            messageToRouter.Append(message[0]);
                            messageToRouter.AppendEmptyFrame();
                            
                            messageToRouter.Append(result[1]);
                            
                            server.SendMultipartMessage(messageToRouter);
                        }
                    }
                    catch (Exception ex)
                    {
                    
                    }
                }
            }
           
            public void Dispose()
            {
                _input?.Dispose();
                _output?.Dispose();
                
                NetMQConfig.Cleanup();
                GC.SuppressFinalize(this);
            }
        }
}