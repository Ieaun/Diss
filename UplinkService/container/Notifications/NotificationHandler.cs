namespace UplinkService.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using UplinkService.Config;
    using UplinkService.Database;
    using UplinkService.Udp;

    public class NotificationHandler : INotificationHandler<Notification>
    {
        private readonly ILogger<NotificationHandler> _Logger;
        private string _PriorityServerIP;
        private int _PriorityServerPort;
        private bool _SendToAllServers;
        private string _TtnIP;
        private int _TtnPort;
        private bool _OnlySendToTtn;
        private readonly IDatabase _Database;
        private readonly IUdpHandler _UdpHandler;

        public NotificationHandler(ILogger<NotificationHandler> logger, IOptions<ContainerSettings> options, IDatabase database, IUdpHandler udpHandler)
        {
            this._Logger = logger;
            this._UdpHandler = udpHandler;
            this._PriorityServerIP = options.Value.PriorityServerIP;
            this._PriorityServerPort = int.Parse(options.Value.PriorityServerPort);
            this._SendToAllServers = bool.Parse(options.Value.SendToAllServers);
            this._TtnIP = options.Value.TtnIP == "" ? Dns.GetHostEntry(options.Value.TtnRouter).AddressList[0].ToString() : options.Value.TtnIP;
            this._TtnPort = int.Parse(options.Value.TtnPort);
            this._OnlySendToTtn = bool.Parse(options.Value.OnlySendToTtn);
        }

        public string SerializePacket(Notification notification) {
            switch (notification.PacketType)
            {
                case "stat":
                    return "{stat:" + Newtonsoft.Json.JsonConvert.SerializeObject(notification.GsMetadata) + "}";
                case "rxpk":
                    return "{rxpk:[" + Newtonsoft.Json.JsonConvert.SerializeObject(notification.RxMetadata) + "]}";
                default:
                    throw new Exception("Unrecognized packet type " + notification.PacketType);
            }
        }

        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            _Logger.LogInformation("Recieved new notification :{@Notification}", notification);
            try
            {
                var unalteredPacketPresent = notification.UnalteredPacket != null;
                var serializedPacket = string.IsNullOrWhiteSpace(notification.OriginalMessage) && !unalteredPacketPresent ? SerializePacket(notification) : notification.OriginalMessage;
                var byteMessage = !unalteredPacketPresent ? JsonSerializer.SerializeToUtf8Bytes(serializedPacket) : notification.UnalteredPacket;

                // send to private server(s)
                if ((notification.isRegisteredDevice || notification.PacketType == "stat") && !_OnlySendToTtn)
                {
                    try
                    {
                        if (_SendToAllServers)
                        {
                            var servers = await _Database.GetAll();
                            servers.ForEach(x => _UdpHandler.Send(x.IpAddress, x.Port, byteMessage));
                        }
                        else
                        {
                            _UdpHandler.Send(_PriorityServerIP, _PriorityServerPort, byteMessage);
                        }
                    }
                    catch (Exception e)
                    {
                        _Logger.LogInformation("Failed sending packet to private server :{@Notification} with exception: {@Exception}", notification, e.Message);
                    }
                }

                // send to the things network
                if ( !notification.isRegisteredDevice || notification.PacketType == "stat") {
                    _UdpHandler.Send(_TtnIP, _TtnPort, byteMessage);
                }                
                
            }
            catch (Exception e)
            {
                _Logger.LogInformation("Failed processing notification :{@Notification} with exception: {@Exception}", notification, e.Message);
                return;
            }

            _Logger.LogInformation("Finished processing notification :{@Notification}", notification);
            return;
        }
    }
}
