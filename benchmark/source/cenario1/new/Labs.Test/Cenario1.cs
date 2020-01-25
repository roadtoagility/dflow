using System;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Xunit;
using Xunit.Abstractions;

namespace Labs.Test
{
    public class Cenario1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Cenario1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ClientDealer_VentilatorRouter_ProcessorsRouter()
        {
            var endpointCommand = "inproc://127.0.0.1:5000";
            var endpointQuery = "inproc://127.0.0.1:5001";
            
            var endpointVentilator = "inproc://127.0.0.1:5100";
            
            var command = new RouterSocket();
            command.ReceiveReady += (sender, args) =>
            {
                _testOutputHelper.WriteLine("New connection: {0}", args.Socket.Options.Identity.ToString());
            };
            
            command.Options.Identity = Encoding.UTF8.GetBytes("COMMAND");
            command.Options.RouterMandatory = true;
            command.Bind(endpointCommand);
            _testOutputHelper.WriteLine("Bind command");

            Task.Delay(1000);
            
            var query = new RouterSocket();
            query.ReceiveReady += (sender, args) =>
            {
                _testOutputHelper.WriteLine("New connection: {0}", args.Socket.Options.Identity.ToString());
            };
            query.Options.Identity = Encoding.UTF8.GetBytes("QUERY");
            query.Options.RouterMandatory = true;
            query.Bind(endpointQuery);
            _testOutputHelper.WriteLine("Bind query");
            
            Task.Delay(2000);

            var ventilatorCommand = new DealerSocket();
            ventilatorCommand.Options.Identity = Encoding.UTF8.GetBytes("VENTILATORCOMMAND");
            ventilatorCommand.Connect(endpointCommand);
            _testOutputHelper.WriteLine("Connect to command");

            Task.Delay(1000);

            VentilatorCommand_SendCommand(ventilatorCommand, command);
            
            Task.Delay(1000);

            var ventilatorQuery = new DealerSocket();
            ventilatorQuery.Options.Identity = Encoding.UTF8.GetBytes("VENTILATORQUERY");
            ventilatorQuery.Connect(endpointQuery);
            _testOutputHelper.WriteLine("Connect to query");
           
            Task.Delay(1000);

            VentilatorQuery_SendQuery(ventilatorQuery, query);
            
            Task.Delay(1000);
            
            var ventilator = new RouterSocket();
            ventilator.Options.Identity = Encoding.UTF8.GetBytes("VENTILATOR");
            ventilator.Bind(endpointVentilator);
            
            _testOutputHelper.WriteLine("Bind Ventilator");
            
            Task.Delay(1000);
            
            var client = new DealerSocket();
            client.Options.Identity = Encoding.UTF8.GetBytes("CLIENT");
            client.Connect(endpointVentilator);
            
            _testOutputHelper.WriteLine("Connect to Ventilator");
            
            Task.Delay(1000);

            ClientCommand_Ventilator(client, ventilator, ventilatorCommand, command);

        }

        void VentilatorCommand_SendCommand( DealerSocket ventilatorCommand, RouterSocket command)
        {
            #region communication ventilatorCommand  to Commander
            
            var msgCommand = new NetMQMessage();
            msgCommand.AppendEmptyFrame();
            msgCommand.Append("HELLO");
            ventilatorCommand.SendMultipartMessage(msgCommand);
            _testOutputHelper.WriteLine("Send Hello to command processor");
            
            Task.Delay(1000);
          
            var messageCommandID = command.ReceiveFrameString();
            command.ReceiveFrameString();
            var messageCommandCommand = command.ReceiveFrameString();
            _testOutputHelper.WriteLine("Received Command: {0} {1}", messageCommandID, messageCommandCommand);
                      
            #endregion
            
        }
        void VentilatorQuery_SendQuery( DealerSocket ventilatorQuery, RouterSocket query)
        {
            #region communication ventilatorQuery  to Querier
            
            var msgQuery = new NetMQMessage();
            msgQuery.AppendEmptyFrame();
            msgQuery.Append("HELLO");
            ventilatorQuery.SendMultipartMessage(msgQuery);
            _testOutputHelper.WriteLine("Send Hello to query processor");

            Task.Delay(1000);
            
            var messageQueryID = query.ReceiveFrameString();
            query.ReceiveFrameString();
            var messageQueryCommand = query.ReceiveFrameString();
            _testOutputHelper.WriteLine("Received Query: {0} {1}", messageQueryID, messageQueryCommand);
            
            #endregion
        }

        void ClientCommand_Ventilator(DealerSocket client, RouterSocket ventilator, DealerSocket ventilatorCommand, RouterSocket command)
        {
            // inicialização das conexões ventilator -> command executor 
            
            // var commandHello = new NetMQMessage();
            // commandHello.AppendEmptyFrame();
            // commandHello.Append("HELLO");
            // ventilatorCommand.SendMultipartMessage(commandHello);
            // _testOutputHelper.WriteLine("Send Hello to command processor");
            //
            // Task.Delay(1000);
            //
            // var hello = command.ReceiveFrameString();
            // command.ReceiveFrameString();
            // var helloCommand = command.ReceiveFrameString();
            // _testOutputHelper.WriteLine("Received Command: {0} {1}", hello, helloCommand);

            // client envia comando 
            
            var commandData = new NetMQMessage();
            commandData.AppendEmptyFrame();
            commandData.Append("COMMAND");
            commandData.Append("COMMAND_DATA");
            client.SendMultipartMessage(commandData);
            _testOutputHelper.WriteLine("Client send a command to ventilator");
            
            Task.Delay(1000);
            
            var senderId = ventilator.ReceiveFrameString();
            ventilator.ReceiveFrameString();
            var requestType = ventilator.ReceiveFrameString();
            var messageCommandData = ventilator.ReceiveFrameString();

            _testOutputHelper.WriteLine("Ventilator Received Request: {0} {1} {2}", senderId, requestType, messageCommandData);
            
            Task.Delay(1000);
            
            // ventilator roteia comando 
            
            var routingCommandData = new NetMQMessage();
            routingCommandData.AppendEmptyFrame();
            routingCommandData.Append(messageCommandData);
            ventilatorCommand.SendMultipartMessage(routingCommandData);
            _testOutputHelper.WriteLine("Ventilator send a command to command processor");
            
            Task.Delay(1000);
            
            var sender = command.ReceiveFrameString();
            command.ReceiveFrameString();
            var receiveCommandData = command.ReceiveFrameString();
            _testOutputHelper.WriteLine("Command Received sender: {0} command args: {1}", sender, receiveCommandData);

            // Task.Delay(1000);
            //
            // var commandAnswerData = new NetMQMessage();
            // commandAnswerData.AppendEmptyFrame();
            // commandAnswerData.Append(messageCommandData + ": OK");
            // command.SendMultipartMessage(commandAnswerData);
            // _testOutputHelper.WriteLine("Command send an answer to ventilator");
            //
            // Task.Delay(1000);
            
            // var senderCommand = ventilatorCommand.ReceiveFrameString();
            // ventilatorCommand.ReceiveFrameString();
            // var receiveCommandAnswer = ventilatorCommand.ReceiveFrameString();
            // _testOutputHelper.WriteLine("Ventilator Received Command Answer: {0} command args: {1}", sender, receiveCommandAnswer);
            //
            // Task.Delay(1000);
            //
            // var routeAnswerData = new NetMQMessage();
            // routeAnswerData.AppendEmptyFrame();
            // routeAnswerData.Append(messageCommandData + ": OK");
            // ventilator.SendMultipartMessage(commandAnswerData);
            // _testOutputHelper.WriteLine("Command send an answer to ventilator");
            //
            // Task.Delay(1000);
            //
            // var senderCommand = ventilatorCommand.ReceiveFrameString();
            // ventilatorCommand.ReceiveFrameString();
            // var receiveCommandAnswer = ventilatorCommand.ReceiveFrameString();
            // _testOutputHelper.WriteLine("Ventilator Received Command Answer: {0} command args: {1}", sender, receiveCommandAnswer);
        }
        
        void ClientQuery_Ventilator(DealerSocket client, RouterSocket ventilator, RouterSocket query)
        {
            
        }
    }
}
