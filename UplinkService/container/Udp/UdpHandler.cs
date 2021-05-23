namespace UplinkService.Udp
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text;
    using SimpleUdp;
    using MediatR;
    

    public class UdpHandler : IUdpHandler
    {
        private readonly ILogger<UdpHandler> _logger;
        private UdpEndpoint _endPoint;
        private readonly IMediator _mediator; 

        public UdpHandler(ILogger<UdpHandler> logger, IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;

            _endPoint = new UdpEndpoint("192.168.8.100", 1700);
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
                    _logger.LogInformation("Uplink UDP server started");
                    return;
                }
                catch (Exception e)
                {
                    _logger.LogError("failed to start to UDP server, retrying {x}/{y}", i + 1, 5);
                    System.Threading.Thread.Sleep(i * 1000);
                }
            }
        }

        public void Send(string address, int port, byte[] message)
        {
            _endPoint.Send(address, port, message);
        }

        public void EndpointDetected(object sender, EndpointMetadata md)
        {
            _logger.LogInformation("Endpoint detected: " + md.Ip + ":" + md.Port);
        }

        public async void DatagramReceived(object sender, Datagram dg)
        {
            _logger.LogInformation("[" + dg.Ip + ":" + dg.Port + "]: " + Encoding.UTF8.GetString(dg.Data));
        }
    }
}
