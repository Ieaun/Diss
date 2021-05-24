namespace LoraWAN_Pipeline.Notifications.ReceivedPackets
{
    using LoraWAN_Pipeline.Notifications.GatewayStatusUpdates;
    using LoraWAN_Pipeline.Notifications.TransmitPackets;
    using MediatR;

    public class NewPacket
    {
        public string PacketType { get; set; }

        public GatewayStatusUpdate StatPacket { get; set; }

        public TransmitPacket TxPacket { get; set; }

        public ReceivedPacket RxPacket { get; set; }

        public byte[] UnalteredPacket { get; set; }
    }
}
