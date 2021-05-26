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
        private readonly IMediator _mediator; 
        private readonly LoRaAbpDecoder _loRaAbpDecoder; 

        public UdpHandler(ILogger<UdpHandler> logger, IQueue queue, IMediator mediator, LoRaAbpDecoder loRaAbpDecoder)
        {
            _logger = logger;
            this._mediator = mediator;
            this._loRaAbpDecoder = loRaAbpDecoder;

            _endPoint = new UdpEndpoint("192.168.8.100", 30099);
            _endPoint.EndpointDetected += EndpointDetected;

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
            _endPoint.Send(address, port, message);
        }

        public void EndpointDetected(object sender, EndpointMetadata md)
        {
            _logger.LogInformation("Endpoint detected: " + md.Ip + ":" + md.Port);
        }

        public async void DatagramReceived(object sender, Datagram dg)
        {
            _logger.LogInformation("Received datagram from [" + dg.Ip + ":" + dg.Port + "]: " + Encoding.UTF8.GetString(dg.Data));

            await _mediator.Publish(new Notification {
                datagram = dg
            });
        }
    }
}
