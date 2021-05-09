namespace LoraWAN_Pipeline.Udp
{
    using LoraWAN_Pipeline.Models;
    using LoraWAN_Pipeline.Queue;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SimpleUdp;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class MessageHandler
    {
        private readonly ILogger<UdpHandler> _logger;
        private readonly IQueue _queue;

        public MessageHandler(ILogger<UdpHandler> logger, IQueue queue)
        {
            this._queue = queue;
            this._logger = logger;
        }

        public async Task Handle(Datagram dg) 
        {
            var data = Encoding.UTF8.GetString(dg.Data);
            var sanitizedData = data.Remove(0, data.IndexOf('{')); 

            var messageType = sanitizedData.Substring(1, 4);
            var message = sanitizedData.Substring(5);
            JObject jsonObj;
            
            try
            {
                jsonObj = JObject.Parse(sanitizedData);
            }
            catch (Exception)
            {
                ParseManually(sanitizedData);
                throw;
            }

            var latitude = jsonObj.First.First["lati"].ToString();

            // determine the type of packet
            LoraPacket packet = new LoraPacket
            {
                Payload = dg.Data,
                OriginalMessage = dg.Data
            };
            await this._queue.EnqueueToUplink(packet);

            //check if from registered device
            var isRegisteredDevice = false;
            if (isRegisteredDevice)
            {
                await this._queue.EnqueueToStorage(packet);
            }
            _logger.LogInformation("[" + dg.Ip + ":" + dg.Port + "] " + Encoding.UTF8.GetString(dg.Data));
        }

        private void ParseManually(string sanitizedData)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUplink() 
        { 

        }

        public async Task HandleStatus()
        {

        }

        public async Task HandleDownlink()
        {

        }
    }
}
