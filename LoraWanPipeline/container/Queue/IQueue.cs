namespace LoraWAN_Pipeline.Queue
{
    using System.Threading.Tasks;
    using LoraWAN_Pipeline.Notifications.ReceivedPackets;

    public interface IQueue
    {
        Task EnqueueToStorage(NewPacket packet);

        Task EnqueueToUplink(NewPacket packet);
    }
}
