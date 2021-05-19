namespace LoraWAN_Pipeline.Notifications
{
    using System;
    using LoraWAN_Pipeline.Queue;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using LoraWAN_Pipeline.ActivationByPersonalization.Decoders;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using LoraWAN_Pipeline.Notifications.ReceivedPackets;
    using LoraWAN_Pipeline.Notifications.GatewayStatusUpdates;
    using LoraWAN_Pipeline.Notifications.TransmitPackets;

    public class NotificationHandler : INotificationHandler<Notification>
    {
        private readonly ILogger<NotificationHandler> _logger;
        private readonly IQueue _queue;
        private readonly GatewayStatusMapper _gatewayStatusMapper;
        private readonly TransmitPacketMapper _transmitPacketMapper;
        private readonly ReceivedPacketMapper _receivedPacketMapper;
        private readonly LoRaAbpDecoder _loRaAbpDecoder;
        
        public NotificationHandler(ILogger<NotificationHandler> logger, IQueue queue, GatewayStatusMapper gatewayStatusMapper,TransmitPacketMapper transmitPacketMapper, ReceivedPacketMapper receivedPacketMapper, LoRaAbpDecoder loRaAbpDecoder)
        {
            this._logger = logger;
            this._queue = queue;
            this._gatewayStatusMapper = gatewayStatusMapper;
            this._transmitPacketMapper = transmitPacketMapper;
            this._receivedPacketMapper = receivedPacketMapper;
            this._loRaAbpDecoder = loRaAbpDecoder;
        }

        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Recieved new notification :{@Notification}", notification);
            try
            {
                var data = Encoding.UTF8.GetString(notification.datagram.Data);
                var sanitizedData = data.Remove(0, data.IndexOf('{'));
                var messageType = sanitizedData.Substring(2, 4);
                var message = sanitizedData.Substring(5);
        
                var jsonObj = JObject.Parse(sanitizedData);

                // determine the type of packet
                switch (messageType)
                {
                    case "stat":
                        var gatewayStatus = _gatewayStatusMapper.Map(jsonObj);
                        break;
                    case "txpk":
                        var transmitPacket = _transmitPacketMapper.Map(jsonObj);
                        break;
                    case "rxpk":
                        sanitizedData = sanitizedData.Remove(0, sanitizedData.IndexOf('['));
                        sanitizedData = sanitizedData.Remove(sanitizedData.Length -1, 1);
                        var receivedPacketsMetadata = JsonConvert.DeserializeObject<List<RecievedPacketMetadata>>(sanitizedData);

                        foreach (var receivePacket in receivedPacketsMetadata)
                        {
                            // used to determine who to send this to in Uplink service
                            // sends to storage service here if true
                            var isRegesteredDevice = await _loRaAbpDecoder.IsRegistedDevice(receivePacket);
                            
                            await this._queue.EnqueueToUplink(new NewPacket
                            {
                                PacketType = "Uplink",
                                Uplink = new ReceivedPacket
                                {
                                    isRegesteredDevice = isRegesteredDevice, //need original message 
                                }
                            });
                        }

                        break;
                    default:
                        break;
                        throw new Exception("cant");
                }               

                _logger.LogInformation("[" + notification.datagram.Ip + ":" + notification.datagram.Port + "] " + Encoding.UTF8.GetString(notification.datagram.Data));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Failed processing notification :{@Notification} with exception: {@Exception}", notification, e.Message);
            }

            _logger.LogInformation("Finished processing notification :{@Notification}", notification);
        }

        private void ParseManually(string sanitizedData)
        {
            throw new NotImplementedException();
        }
    }
}
