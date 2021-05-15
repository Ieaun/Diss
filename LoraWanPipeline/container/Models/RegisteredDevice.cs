namespace LoraWAN_Pipeline
{
    public class RegisteredDevice
    {
        public int Id { get; set; }

        public byte[] NetworkSessionKey { get; set; }

        public string DeviceAddress { get; set; }
    }
}
