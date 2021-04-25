namespace DownlinkService.Queue
{
    using System.Threading.Tasks;
    using DownlinkService.Notifications;
    using QueueTypes.Models;

    public interface IQueue
    {
       void OnHandleNotification(QueueTypes.Queues.Uplink packet);
    }
}
