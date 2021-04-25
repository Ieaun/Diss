namespace DownlinkService.Queue
{
    public interface IQueue
    {
       void OnHandleNotification(QueueTypes.Queues.Downlink packet);
    }
}
