namespace QueueTypes.Queues
{
    using EasyNetQ;
    using QueueTypes.Models;

    [Queue("Uplink", ExchangeName = "LoRaWAN Exchange")]
    public class Uplink
    {
        public NewPacket Packet { get; set; }
    }
}
