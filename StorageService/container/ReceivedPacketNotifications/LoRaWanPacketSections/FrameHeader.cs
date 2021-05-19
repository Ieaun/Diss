namespace StorageService.ReceivedPacketNotifications.LoRaWanPacketSections
{
    public class FrameHeader
    {
        public string DeviceAddress { get; set; }

        public string FrameControlOctet { get; set; }

        public string FrameCounter { get; set; }

        // optional
        public string FrameOptions { get; set; }
    }
}