﻿namespace QueueTypes.Queues.Uplinks
{
    using EasyNetQ;
    using QueueTypes.Models.GatewayStatusUpdates;
    using QueueTypes.Models.ReceivedPackets;

    [Queue("Uplink", ExchangeName = "LoRaWAN Exchange")]
    public class Uplink
    {
        public bool isRegesteredDevice { get; set; }

        public ReceivedPacket RxPacket { get; set; }

        public GatewayStatusUpdate GsPacket { get; set; }

        public string PacketType { get; set; } 
    }
}
