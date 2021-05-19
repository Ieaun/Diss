namespace LoraWAN_Pipeline.Notifications.ReceivedPackets
{
    using LoraWAN_Pipeline.Models;

    public class ReceivedPacket : ISemtechUdpPacket
    {
        public RecievedPacketMetadata metadata { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public bool isRegesteredDevice { get; set; }

        public byte[] OriginalMessage { get; set; }

        public string PacketType { get; } = "Uplink";
    }
}
