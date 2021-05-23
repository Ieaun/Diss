namespace UplinkService.Queue
{
    using System.Threading.Tasks;
    using UplinkService.Notifications;
    using QueueTypes.Models;

    public interface IQueue
    {
       void OnHandleNotification(QueueTypes.Queues.Uplinks.Uplink packet);
    }
}
