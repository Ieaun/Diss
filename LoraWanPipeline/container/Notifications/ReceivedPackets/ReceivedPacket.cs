namespace LoraWAN_Pipeline.Notifications.ReceivedPackets
{
    using LoraWAN_Pipeline.Models;

    public class ReceivedPacket : ISemtechUdpPacket
    {
        public ReceivedPacketMetadata metadata { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public bool isRegesteredDevice { get; set; }
    }
}
