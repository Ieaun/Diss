namespace LoraWAN_Pipeline.Models
{
    public class LoRaDevice
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public object LastMessageSent { get; set; }
    }
}
