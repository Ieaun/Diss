namespace StorageService.Notifications.ReceivedPackets
{
    using StorageService.Notifications.GatewayStatusUpdates;
    using StorageService.Notifications.TransmitPackets;
    using MediatR;

    public class NewPacket
    {
        public int Id { get; set; }

        public string PacketType { get; set; }

        public GatewayStatusUpdate Status { get; set; }

        public TransmitPacket Downlink { get; set; }

        public ReceivedPacket Uplink { get; set; }
    }
}
