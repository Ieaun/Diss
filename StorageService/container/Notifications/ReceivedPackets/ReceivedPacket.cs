namespace StorageService.Notifications.ReceivedPackets
{
    using StorageService.ReceivedPacketNotifications;

    public class ReceivedPacket : ISemtechUdpPacket
    {
        public RecievedPacketMetadata metadata { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public bool isRegistered { get; set; }

        public string OriginalMessage { get; set; }

        public string PacketType { get; } = "Uplink";
    }
}
