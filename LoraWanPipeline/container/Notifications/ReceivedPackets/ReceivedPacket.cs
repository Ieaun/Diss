namespace LoraWAN_Pipeline.Notifications.ReceivedPackets
{
    using LoraWAN_Pipeline.Models;

    public class ReceivedPacket : ISemtechUdpPacket
    {
        public RecievedPacketMetadata metadata { get; set; }

        public SemtechUplinkHeaderMetaData semtechHeader { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public string OriginalMessage { get; set; }

        public bool isRegesteredDevice { get; set; }

        public string PacketType { get; } = "Uplink";
    }
}
