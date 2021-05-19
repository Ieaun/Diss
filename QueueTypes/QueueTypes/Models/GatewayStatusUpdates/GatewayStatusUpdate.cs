namespace QueueTypes.Models.GatewayStatusUpdates
{

    public class GatewayStatusUpdate : ISemtechUdpPacket
    {
        public GatewayStatusUpdate metadata { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public bool isRegesteredDevice { get; set; }

        public string PacketType { get; } = "Status";
    }
}
