namespace StorageService.Notifications.GatewayStatusUpdates
{
    using StorageService.ReceivedPacketNotifications;

    public class GatewayStatusUpdate : ISemtechUdpPacket
    {
        public GatewayStatusUpdate metadata { get; set; }

        public string OriginalMessage { get; set; }

        public string PacketType { get; } = "Status";
    }
}
