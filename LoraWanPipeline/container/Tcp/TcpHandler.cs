using DownlinkService.Database;

namespace LoraWAN_Pipeline.Tcp
{
    using LoraWAN_Pipeline.Queue;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text;
    using SimpleTcp;

    public class TcpHandler : ITcpHandler
    {
        private readonly ILogger<TcpHandler> _logger;
        private readonly IQueue _queue;
        private SimpleTcpServer _client;
        private readonly IDatabase _database;

        public TcpHandler(ILogger<TcpHandler> logger, IQueue queue)//, IDatabase database)
        {
            this._logger = logger;
            this._queue = queue;
            //this._database = database;

            _client = new SimpleTcpServer("0.0.0.0:30099");

            // set events
            _client.Events.ClientConnected += Connected;
            _client.Events.ClientDisconnected += Disconnected;
            _client.Events.DataReceived += DataReceived;
        }

        public void Start()
        {
            for (int i = 0; i < 50; i++)
            {
                try
                {
                    _client.Start();
                    i = 5;
                    return;
                }
                catch (Exception e)
                {
                    _logger.LogInformation("failed to connect to LoRa hardware, try {x}/{y}", i+1,5);
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
            LoraPacket packet = new LoraPacket
            {
                Payload = e.Data
            };
            await this._queue.EnqueueToUplink(packet);

            //check if from registered device
            var isRegisteredDevice = false;
            if (isRegisteredDevice)
            {
                await this._queue.EnqueueToStorage(packet);
            }
            _logger.LogInformation("[" + e.IpPort + "] " + Encoding.UTF8.GetString(e.Data));
        }
    }
}
