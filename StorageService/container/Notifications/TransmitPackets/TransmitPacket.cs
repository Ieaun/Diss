namespace StorageService.Notifications.TransmitPackets
{
    using StorageService.ReceivedPacketNotifications;

    public class TransmitPacket : ISemtechUdpPacket
    {
        public TransmitPacketMetadata metadata { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public string PacketType { get; } = "Downlink";
    }
}
