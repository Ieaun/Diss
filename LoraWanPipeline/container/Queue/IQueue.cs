namespace LoraWAN_Pipeline.Queue
{
    using System.Threading.Tasks;
    using LoraWAN_Pipeline.Tcp;
    using LoraWAN_Pipeline.Notifications;
    public interface IQueue
    {
        Task EnqueueToStorage(LoraPacket packet);
        Task EnqueueToUplink(LoraPacket packet);
        Task EnqueueToDownlink(Notification notification);
    }
}
