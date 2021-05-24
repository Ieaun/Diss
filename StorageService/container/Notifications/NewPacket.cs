namespace StorageService.Notifications.ReceivedPackets
{
    using MongoDB.Bson.Serialization.Attributes;
    using StorageService.Notifications.GatewayStatusUpdates;
    using StorageService.Notifications.TransmitPackets;

    [BsonIgnoreExtraElements]
    public class NewPacket
    {
        public string PacketType { get; set; }

        public GatewayStatusUpdate Status { get; set; }

        public TransmitPacket Downlink { get; set; }

        public ReceivedPacket Uplink { get; set; }
    }
}
