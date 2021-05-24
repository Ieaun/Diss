namespace LoraWAN_Pipeline.Notifications.GatewayStatusUpdates
{
    using LoraWAN_Pipeline.Models;

    public class GatewayStatusUpdate : ISemtechUdpPacket
    {
        public GatewayStatusUpdateMetadata metadata { get; set; }

        public SemtechUplinkHeaderMetaData semtechHeader { get; set; }

        public string OriginalMessage { get; set; }

        public string PacketType { get; } = "Status";
    }
}
