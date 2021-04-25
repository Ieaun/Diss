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
        private SimpleTcpClient _client;
        public TcpHandler(ILogger<TcpHandler> logger, IQueue queue)
        {
            this._logger = logger;
            this._queue = queue;

            _client = new SimpleTcpClient("127.0.0.1:30099");

            // set events
            _client.Events.Connected += Connected;
            _client.Events.Disconnected += Disconnected;
            _client.Events.DataReceived += DataReceived;
        }

        public void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    _client.Connect();
                    i = 5;
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
