namespace StorageService.Notifications
{
    using MediatR;
    public class Notification: INotification
    {
        public int Id { get; set; }
        public object Payload { get; set; }
    }
}
