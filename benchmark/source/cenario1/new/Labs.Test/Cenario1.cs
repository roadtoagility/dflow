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
                      
            Task.Delay(1000);

            var ventilatorQuery = new DealerSocket();
            ventilatorQuery.Options.Identity = Encoding.UTF8.GetBytes("VENTILATORQUERY");
            ventilatorQuery.Connect(endpointQuery);
            _testOutputHelper.WriteLine("Connect to query");
           
            Task.Delay(1000);
            
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
            
            Task.Delay(1000);
            
            var ventilator = new RouterSocket();
            ventilator.Options.Identity = Encoding.UTF8.GetBytes("VENTILATOR");
            ventilator.Bind(endpointVentilator);
            _testOutputHelper.WriteLine("Bind Ventilator");
        }
    }
}
