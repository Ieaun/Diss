namespace LoraWAN_Pipeline.Notifications
{
    using MediatR;
    using SimpleUdp;

    public class Notification: INotification
    {
        public Datagram datagram { get; init; } 
    }
}
