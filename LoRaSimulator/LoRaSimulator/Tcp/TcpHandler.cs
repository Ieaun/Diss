namespace LoRaSimulator.Tcp
{
    using Microsoft.Extensions.Logging;
    using System.Text;
    using SimpleTcp;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class TcpHandler : ITcpHandler
    {
        private readonly ILogger<TcpHandler> _logger;
        private SimpleTcpServer _server;
        private List<string> _tcpClients = new List<string>();

        public TcpHandler(ILogger<TcpHandler> logger)
        {
            this._logger = logger;
        }

        public void Start()
        {
            _logger.LogInformation("Starting TcpHandler");
            // instantiate
            _server = new SimpleTcpServer("127.0.0.1:30099");

            // set events
            _server.Events.ClientConnected += ClientConnected;
            _server.Events.ClientDisconnected += ClientDisconnected;
            _server.Events.DataReceived += DataReceived;

            _server.Start();
        }

        public void Send(LoraPacket packet)
        {
            foreach (var client in _tcpClients)
            {
                _logger.LogInformation( "Sending Uplink to client {@client}", client);
                string json = JsonConvert.SerializeObject(packet);
                _server.Send(client, json);
            }
        }

        public void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            _logger.LogInformation("[" + e.IpPort + "] client connected");
            _tcpClients.Add(e.IpPort);
        }

        public void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            _logger.LogInformation("[" + e.IpPort + "] client disconnected: " + e.Reason.ToString());
            _tcpClients.Remove(e.IpPort);
        }

        public void DataReceived(object sender, DataReceivedEventArgs e)
        {
            _logger.LogInformation("Downlink Received from: [" + e.IpPort + "]: " + Encoding.UTF8.GetString(e.Data));
        }
    }
}
