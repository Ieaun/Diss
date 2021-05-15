namespace LoraWAN_Pipeline.Models.LoRaWanPacketSections
{
    public class FrameHeader
    {
        public string OriginalHexString { get; set; }

        public string DeviceAddress { get; set; }

        public string FrameControlOctet { get; set; }

        public string FrameCounter { get; set; }

        // optional
        public string FrameOptions { get; set; }
    }
}