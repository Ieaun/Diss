namespace LoraWAN_Pipeline.Models.LoRaWanPacketSections
{
    public class PhysicalPayload
    {
        public string OriginalHexString { get; set; }

        public string MacHeader { get; set; }

        public MacPayload MacPayload { get; set; }

        public string MIC { get; set; }
    }
}
