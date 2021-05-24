﻿namespace QueueTypes.Models.ReceivedPackets
{
    public class ReceivedPacket : ISemtechUdpPacket
    {
        public ReceivedPacketsMetadata metadata { get; set; }

        public DecodedLoraPacket decodedPacket { get; set; }

        public bool isRegistered { get; set; }

        public byte[] OriginalMessage { get; set; }

        public string PacketType { get; } = "Uplink";
    }
}
