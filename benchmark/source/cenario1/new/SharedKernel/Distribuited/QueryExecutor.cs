using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace SharedKernel.Distribuited
{
    public abstract class QueryExecutor<TQuery, TResult> : ITransport<RouterSocket, DealerSocket>
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

             public abstract TResult Handle(TQuery query);
             
             private void StartExternalEndpoint()
             {
                 using (var server = new RouterSocket())
                 {
                     server.Options.Identity = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                     server.Options.RouterMandatory = true;
                     server.Bind($"{_externalEndpoint}");
                  
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
                             
                             var response = Handle(JsonSerializer.Deserialize<TQuery>(json)); 
                        
                             var messageToRouter = new NetMQMessage();
                              messageToRouter.Append(message[0]);
                              messageToRouter.AppendEmptyFrame();
                              messageToRouter.Append(JsonSerializer.Serialize(response));

                             server.SendMultipartMessage(messageToRouter);
                         }
                     }
                     catch (Exception ex)
                     {
                    
                     }
                 }
             }

             public void Start()
             {
                 throw new NotImplementedException();
             }

             public void Dispose()
             {
                 _input?.Dispose();
                 _output?.Dispose();
             }
         }
}