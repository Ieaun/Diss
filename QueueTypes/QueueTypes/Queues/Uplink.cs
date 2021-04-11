namespace QueueTypes.Queues
{
    using QueueTypes.Models;
    using EasyNetQ;

    [Queue("Uplink", ExchangeName = "LoRaWAN Exchange")]
    public class Uplink
    {
        public LoraPacket Packet { get; set; }
    }
}
