namespace UplinkService.Models.GatewayStatusUpdates
{

    public class GatewayStatusUpdate : ISemtechUdpPacket
    {
        public GatewayStatusUpdateMetadata metadata { get; set; }

        public SemtechUplinkHeaderMetaData semtechHeader { get; set; }

        public byte[] OriginalMessage { get; set; }

        public string PacketType { get; } = "Status";
    }
}
