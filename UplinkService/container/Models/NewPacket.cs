namespace UplinkService.Models
{
    using UplinkService.Models.GatewayStatusUpdates;
    using UplinkService.Models.ReceivedPackets;

    public class NewPacket
    {
        public string PacketType { get; set; }

        public GatewayStatusUpdate Status { get; set; }

        public ReceivedPacket Uplink { get; set; }

        public byte[] UnalteredPacket { get; set; }
    }
}
