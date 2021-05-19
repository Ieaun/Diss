namespace StorageService.ReceivedPacketNotifications
{
    using StorageService.ReceivedPacketNotifications.LoRaWanPacketSections;

    public class DecodedLoraPacket
    {
        public string OriginalHexString { get; set; }

        public PhysicalPayload PhysicalPayload { get; set; }
    }
}
