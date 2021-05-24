namespace UplinkService.Notifications
{
    using MediatR;
    public class Notification: INotification
    {
        public string PacketType { get; set; }

        public bool isRegisteredDevice { get; set; }

        public ReceivedPacketMetadata RxMetadata { get; set; }

        public GatewayStatusUpdateMetadata GsMetadata { get; set; }

        public string OriginalMessage { get; set; }

        public byte[] UnalteredPacket { get; set; }
    }
}
