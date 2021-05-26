namespace LoRaSimulator.Tcp
{
    using Microsoft.Extensions.Logging;
    using System.Text;
    using SimpleUdp;
    using System.IO;

    public class UdpHandler : IUdpHandler
    {
        private readonly ILogger<UdpHandler> _logger;
        private UdpEndpoint _endpoint;

        public UdpHandler(ILogger<UdpHandler> logger)
        {
            this._logger = logger;
        }

        public void Start()
        {
            _logger.LogInformation("Starting Udp Handler");
            _endpoint = new UdpEndpoint("192.168.8.100", 30098);
            _endpoint.EndpointDetected += EndpointDetected;

           
            _endpoint.DatagramReceived += DatagramReceived;
            _endpoint.StartServer();
        }

        public void Send(string message)
        {
            _endpoint.Send("192.168.8.100", 30099, message);
        }

        public void EndpointDetected(object sender, EndpointMetadata md)
        {
            _logger.LogInformation("Endpoint detected: " + md.Ip + ":" + md.Port);
        }

        public void DatagramReceived(object sender, Datagram dg)
        {
            _logger.LogInformation("[" + dg.Ip + ":" + dg.Port + "]: " + Encoding.UTF8.GetString(dg.Data));
        }
    }
}
