namespace QueueTypes.Models
{
    using QueueTypes.Models.LoRaWanPacketSections;

    public class DecodedLoraPacket
    {
        public string OriginalHexString { get; set; }

        public PhysicalPayload PhysicalPayload { get; set; }
    }
}
