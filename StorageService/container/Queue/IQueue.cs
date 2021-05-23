namespace StorageService.Queue
{
    public interface IQueue
    {
       void OnHandleNotification(QueueTypes.Queues.Storages.Storage packet);
    }
}
