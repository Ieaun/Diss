namespace QueueTypes.Models.LoRaWanPacketSections
{
    public class PhysicalPayload
    {
        public string MacHeader { get; set; }

        public MacPayload MacPayload { get; set; }

        public string MIC { get; set; }
    }
}
