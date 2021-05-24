namespace QueueTypes.Models.ReceivedPackets
{
    public class ReceivedPacket : ISemtechUdpPacket
    {
        public ReceivedPacketsMetadata metadata { get; set; }

        public SemtechUplinkHeaderMetaData semtechHeader { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public bool isRegistered { get; set; }

        public string OriginalMessage { get; set; }

        public string PacketType { get; } = "Uplink";
    }
}
