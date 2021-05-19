namespace QueueTypes.Models.TransmitPackets
{
    public class TransmitPacket : ISemtechUdpPacket
    {
        public TransmitPacketMetadata metadata { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public string PacketType { get; } = "Downlink";
    }
}
