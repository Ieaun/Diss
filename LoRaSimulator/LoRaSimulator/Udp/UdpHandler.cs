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
            _endpoint = new UdpEndpoint("127.0.0.1", 8000);
            _endpoint.EndpointDetected += EndpointDetected;

            // only if you want to receive messages...
            _endpoint.DatagramReceived += DatagramReceived;
            _endpoint.StartServer();
        }

        public void Send()
        {
            var message = File.ReadAllText("stat.txt");
            _endpoint.Send("127.0.0.1", 8001, "Hello to my friend listening on port 8001!");
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
