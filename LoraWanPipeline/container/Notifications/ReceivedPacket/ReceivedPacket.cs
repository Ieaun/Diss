namespace LoraWAN_Pipeline.Notifications.ReceivedPacket
{
    public class ReceivedPacket : ISemtechUdpPacket
    {
        public ReceivedPacketMetadata metadata { get; set; }

        public bool isRegesteredDevice { get; set; }
    }
}
