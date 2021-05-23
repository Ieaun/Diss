namespace QueueTypes.Queues.Downlinks
{
    using EasyNetQ;
    using QueueTypes.Models;

    [Queue("DownlinkQueue", ExchangeName = "LoRaWAN Exchange")]
    public class Downlink
    {
        public NewPacket Packet { get; set; }
    }
}
