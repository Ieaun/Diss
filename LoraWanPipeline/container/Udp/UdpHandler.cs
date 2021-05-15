namespace LoraWAN_Pipeline.Udp
{
    using LoraWAN_Pipeline.Queue;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text;
    using SimpleUdp;
    using DownlinkService.Database;
    using MediatR;
    using LoraWAN_Pipeline.Notifications;
    using LoraWAN_Pipeline.ActivationByPersonalization.Decoders;
    

    public class UdpHandler : IUdpHandler
    {
        private readonly ILogger<UdpHandler> _logger;
        private UdpEndpoint _endPoint;
        private readonly IDatabase _database;
        private readonly IMediator _mediator; 
        private readonly LoRaAbpDecoder _loRaAbpDecoder; 

        public UdpHandler(ILogger<UdpHandler> logger, IQueue queue, IMediator mediator, LoRaAbpDecoder loRaAbpDecoder)
        {
            _logger = logger;
            this._mediator = mediator;
            this._loRaAbpDecoder = loRaAbpDecoder;

            _endPoint = new UdpEndpoint("192.168.8.100", 30099);
            _endPoint.EndpointDetected += EndpointDetected;

            // only if you want to receive messages...
            _endPoint.DatagramReceived += DatagramReceived;

            var key = new byte[]{
                0xB3, 0x38, 0xEB, 0xD8, 0x72, 0x2A, 0x76, 0x4E, 0xBD, 0x9E, 0x9D, 0xF0, 0x4D, 0x7C, 0xCA, 0xE1
            };

            var cipherText = @"QCIQASaAAAABz+ZtENUq9UCUIgrynehqYt8=";

            _loRaAbpDecoder.DecodePhysicalPayload(cipherText);

            string heree = "";
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

            await _mediator.Publish(new Notification {
                datagram = dg
            });
        }
    }
}
