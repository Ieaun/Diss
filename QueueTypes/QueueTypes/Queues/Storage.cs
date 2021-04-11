namespace QueueTypes.Queues
{
    using EasyNetQ;
    using QueueTypes.Models;

    [Queue("Storage", ExchangeName = "LoRaWAN Exchange")]
    public class Storage
    {
        public LoraPacket Packet { get; set; }
    }
}
