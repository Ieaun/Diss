namespace StorageService.ReceivedPacketNotifications
{
    using Newtonsoft.Json;

    public class TransmittedPacketMetadata : ISemtechUdpPacket
    {
        // Descriptions taken from: https://github.com/Lora-net/packet_forwarder/blob/master/PROTOCOL.TXT

        public DecodedLoraPacket packet { get; set; }
    }
}
