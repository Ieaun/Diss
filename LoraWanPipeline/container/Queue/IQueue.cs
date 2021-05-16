namespace LoraWAN_Pipeline.Queue
{
    using System.Threading.Tasks;
    using LoraWAN_Pipeline.Tcp;
    using LoraWAN_Pipeline.Notifications;
    using LoraWAN_Pipeline.Models;
    using LoraWAN_Pipeline.Notifications.ReceivedPackets;

    public interface IQueue
    {
        Task EnqueueToStorage(ISemtechUdpPacket packet);
        Task EnqueueToUplink(ReceivedPacket packet);
    }
}
