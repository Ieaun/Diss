namespace QueueTypes.Queues
{
    using QueueTypes.Models;
    using EasyNetQ;

    [Queue("DownlinkQueue", ExchangeName = "LoRaWAN Exchange")]
    public class Downlink
    {
        public LoraPacket Packet { get; set; }
    }
}
