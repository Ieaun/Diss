namespace LoraWAN_Pipeline.Notifications.ReceivedPackets
{
    using LoraWAN_Pipeline.Notifications.GatewayStatusUpdates;
    using LoraWAN_Pipeline.Notifications.TransmitPackets;
    using MediatR;

    public class NewPacket
    {
        public string PacketType { get; set; }

        public GatewayStatusUpdate Status { get; set; }

        public TransmitPacket Downlink { get; set; }

        public ReceivedPacket Uplink { get; set; }
    }
}
