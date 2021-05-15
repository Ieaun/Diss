namespace LoraWAN_Pipeline.Models
{
    using LoraWAN_Pipeline.Models.LoRaWanPacketSections;

    public class DecodedLoraPacket
    {
        public string OriginalHexString { get; set; }

        public PhysicalPayload PhysicalPayload { get; set; }
    }
}
