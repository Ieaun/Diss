namespace QueueTypes.Models.GatewayStatusUpdates
{

    public class GatewayStatusUpdate : ISemtechUdpPacket
    {
        public GatewayStatusUpdateMetadata metadata { get; set; }

        public string OriginalMessage { get; set; }

        public string PacketType { get; } = "Status";
    }
}
