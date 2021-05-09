namespace LoraWAN_Pipeline.Udp
{
    using LoraWAN_Pipeline.Queue;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text;
    using SimpleUdp;
    using DownlinkService.Database;
    using LoraWAN_Pipeline.Models;

    public class UdpHandler : IUdpHandler
    {
        private readonly ILogger<UdpHandler> _logger;
        private UdpEndpoint _endPoint;
        private readonly IDatabase _database;
        private MessageHandler _messageHandler;

        public UdpHandler(ILogger<UdpHandler> logger, IQueue queue, MessageHandler messageHandler)//, IDatabase database)
        {
            _logger = logger;
            this._messageHandler = messageHandler;
            //this._database = database;

            _endPoint = new UdpEndpoint("192.168.8.100", 30099);
            _endPoint.EndpointDetected += EndpointDetected;

            // only if you want to receive messages...
            _endPoint.DatagramReceived += DatagramReceived;
        }

        public void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    _endPoint.StartServer();
                    _logger.LogInformation("LoRaWAN Pipeline UDP server started");
                    return;
                }
                catch (Exception e)
                {
                    _logger.LogError("failed to start to UDP server, retrying {x}/{y}", i + 1, 5);
                    System.Threading.Thread.Sleep(i * 1000);
                }
            }
        }

        public void Send(string address, int port, string message)
        {
            // send a message...
            _endPoint.Send(address, port, message);
        }

        public void EndpointDetected(object sender, EndpointMetadata md)
        {
            _logger.LogInformation("Endpoint detected: " + md.Ip + ":" + md.Port);
        }

        public async void DatagramReceived(object sender, Datagram dg)
        {
            _logger.LogInformation("[" + dg.Ip + ":" + dg.Port + "]: " + Encoding.UTF8.GetString(dg.Data));
            await _messageHandler.Handle(dg);
        }
    }
}
