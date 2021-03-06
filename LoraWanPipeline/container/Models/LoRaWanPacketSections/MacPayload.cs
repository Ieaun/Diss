namespace LoraWAN_Pipeline.Models.LoRaWanPacketSections
{
    public class MacPayload
    {
        public FrameHeader FrameHeader { get; set; }

        // this is encrypted, cant use unless you know the keys
        public string FramePayload { get; set; }

        // optional
        public string FramePort { get; set; }
    }
}
