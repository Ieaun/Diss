namespace LoraWAN_Pipeline.Notifications.TransmitPackets
{
    using Newtonsoft.Json;

    public class TransmitPacketMetadata
    {
        public string Time { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public int Rxnb { get; set; }

        public int Rxok { get; set; }

        public int Rxfw { get; set; }

        public double Ackr { get; set; }

        public int Dwnb { get; set; }

        public int Txnb { get; set; }

        public string pfrm { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }
    }
}
