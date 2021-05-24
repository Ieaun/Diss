namespace QueueTypes.Models
{
    using QueueTypes.Models.GatewayStatusUpdates;
    using QueueTypes.Models.ReceivedPackets;
    using QueueTypes.Models.TransmitPackets;

    public class NewPacket
    {
        public string PacketType { get; set; }

        public GatewayStatusUpdate Status { get; set; }

        public TransmitPacket Downlink { get; set; }

        public ReceivedPacket Uplink { get; set; }

        public byte[] UnalteredPacket { get; set; }
    }
}
