namespace UplinkService.Models.ReceivedPackets
{
    public class ReceivedPacket : ISemtechUdpPacket
    {
        public bool isRegesteredDevice { get; set; }

        public byte[] OriginalMessage { get; set; }

        public string PacketType { get; } = "Uplink";
    }
}
