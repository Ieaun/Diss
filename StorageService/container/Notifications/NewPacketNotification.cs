namespace StorageService.Notifications
{
    using MediatR;
    using StorageService.Notifications.ReceivedPackets;

    public class NewPacketNotification : INotification
    {
        public NewPacket Packet { get; set; }
    }
}
