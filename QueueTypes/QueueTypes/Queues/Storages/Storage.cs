namespace QueueTypes.Queues.Storages
{
    using EasyNetQ;
    using QueueTypes.Models;

    [Queue("Storage", ExchangeName = "LoRaWAN Exchange")]
    public class Storage
    {
        public NewPacket Packet { get; set; }
    }
}
