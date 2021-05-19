namespace LoraWAN_Pipeline.Notifications.TransmitPackets
{
    using LoraWAN_Pipeline.Models;

    public class TransmitPacket : ISemtechUdpPacket
    {
        public TransmitPacketMetadata metadata { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public string PacketType { get; } = "Downlink";
    }
}
