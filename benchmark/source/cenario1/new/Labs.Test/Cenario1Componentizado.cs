using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Labs.Test
{
    public class Cenario1Componentizado
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Cenario1Componentizado(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ClientDealer_VentilatorRouter_ProcessorsRouter()
        {
            var endpointCommand = "inproc://127.0.0.1:5000";
            var endpointQuery = "inproc://127.0.0.1:5001";
            
            var endpointVentilator = "inproc://127.0.0.1:5100";
            

            var queryExecutor = new QueryExecutor(endpointQuery,null);

            Task.Delay(1000);
            
            var queryHandler = new QueryHandler(endpointVentilator,endpointQuery);
            
            Task.Delay(5000);
            
            var client = new ClientHandler(endpointVentilator);
            
            Task.Delay(1000);
            
            client.Send("TESTE");
            
             
        }
        
        interface ITransport<TInputSocket, TOutputSocket>: IDisposable 
                                where TInputSocket: NetMQSocket 
                                where TOutputSocket: NetMQSocket
        {
           
        }

         interface IOutputTransport<TData>
         {
             void Send(TData data);
         }

         public sealed class ClientHandler : IOutputTransport<String>
         {
             private readonly String _serverEndpoint;

             public ClientHandler(String serverEndpoint)
             {
                 _serverEndpoint = serverEndpoint;
             }
             
             public void Send(String message)
             {
                 using (var server = new DealerSocket())
                 {
                     server.Options.Identity = Encoding.UTF8.GetBytes("CLIENT");

                     var ok = false;
                     do
                     {
                         try
                         {

                             server.Connect(_serverEndpoint);
                             ok = true;
                             Task.Delay(1000);

                         }
                         catch (NetMQException ex)
                         {
                             
                         }
                         
                     } while (!ok);
                     
                     
                     var msg = new NetMQMessage();
                     msg.AppendEmptyFrame();
                     msg.Append(message);

                     server.SendMultipartMessage(msg);
                     
                     var result = server.ReceiveMultipartMessage();

                     var te = result[0];
                 }
             }
         }

         public sealed class QueryExecutor : ITransport<RouterSocket, DealerSocket>
         {
             private readonly RouterSocket _input;
             private readonly DealerSocket _output;

             private readonly String _externalEndpoint;
             private readonly String _statusEndpoint;

             public QueryExecutor(String externalEndpoint, String statusEndpoint)
             {
                 _externalEndpoint = externalEndpoint;
                 _statusEndpoint = statusEndpoint;
                 
                 var server = new Task(() => StartExternalEndpoint(),
                     TaskCreationOptions.LongRunning);
                
                 server.Start();
             }
             
             private void StartExternalEndpoint()
             {
                 using (var server = new RouterSocket())
                 {
                     server.Options.Identity = Encoding.UTF8.GetBytes("QUERYEXECUTOR");
                     server.Options.RouterMandatory = true;
                     server.Bind(_externalEndpoint);
                  
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

                             var json = message[2].ConvertToString();
                            

                             //TODO: Executar a query e retornar
                            
                             // var query = JsonSerializer.Deserialize<TQuery>(json);
                             // var result = Handler(query);
                        
                             var messageToRouter = new NetMQMessage();
                              messageToRouter.Append(message[0]);
                              messageToRouter.AppendEmptyFrame();
                              messageToRouter.Append("OK");
                             // messageToRouter.Append(JsonSerializer.Serialize(result));

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
             }
         }
         
        

        // public abstract class Transport: ITransport<RouterSocket, DealerSocket>
        // {
        //     private readonly RouterSocket _input;
        //     private readonly DealerSocket _output;
        //
        //     private readonly String _externalEndpoint;
        //     
        //     // protected abstract NetMQMessage  
        // }

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
                
                var server = new Task(StartExternalEndpoint,_externalEndpoint,
                    CancellationToken.None, TaskCreationOptions.LongRunning);
                
                server.Start();                
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
            
            private void StartExternalEndpoint(object? endpoint)
            {
                using (var server = new RouterSocket())
                {
                    server.Options.Identity = Encoding.UTF8.GetBytes("QUERYHANDLER");
                    server.Options.RouterMandatory = true;
                    server.Bind(endpoint.ToString());
                    var ok = false;

                    do
                    {
                        try
                        {
                            _output.Connect(_internalEndpoint);
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
                            
                            // var query = JsonSerializer.Deserialize<TQuery>(json);
                            // var result = Handler(query);
                            
                            var message1 = new NetMQMessage();
                            message1.Append(message[0]);
                            message1.AppendEmptyFrame();
                            message1.Append(message[2]);
                            
                            _output.SendMultipartMessage(message1);

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
}
