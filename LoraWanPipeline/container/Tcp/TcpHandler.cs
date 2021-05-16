namespace LoraWAN_Pipeline.Tcp
{
    using LoraWAN_Pipeline.Queue;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text;
    using SimpleTcp;
    using DownlinkService.Database;
    using LoraWAN_Pipeline.Models;

    public class TcpHandler : ITcpHandler
    {
        private readonly ILogger<TcpHandler> _logger;
        private readonly IQueue _queue;
        private SimpleTcpServer _server;

        public TcpHandler(ILogger<TcpHandler> logger, IQueue queue)//, IDatabase database)
        {
            this._logger = logger;
            this._queue = queue;

            _server = new SimpleTcpServer("0.0.0.0:30099");

            // set events
            _server.Events.ClientConnected += Connected;
            _server.Events.ClientDisconnected += Disconnected;
            _server.Events.DataReceived += DataReceived;
        }

        public void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    _server.Start();
                    _logger.LogInformation("LoRaWAN Pipeline server started");
                    return;
                }
                catch (Exception e)
                {
                    _logger.LogError("failed to connect to LoRa hardware, try {x}/{y}", i+1,5);
                    System.Threading.Thread.Sleep(i * 1000);
                }
            }
        }

        public void Connected(object sender, EventArgs e)
        {
            _logger.LogInformation("*** Server connected");
        }

        public void Disconnected(object sender, EventArgs e)
        {
            _logger.LogInformation("*** Server disconnected");
        }

        public async void DataReceived(object sender, DataReceivedEventArgs e)
        {
      
            _logger.LogInformation("[" + e.IpPort + "] " + Encoding.UTF8.GetString(e.Data));
        }
    }
}
