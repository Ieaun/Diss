namespace LoRaSimulator.Models
{
    public class GatewayStatusUpdate 
    {
        // Descriptions taken from: https://github.com/Lora-net/packet_forwarder/blob/master/PROTOCOL.TXT

        // UTC 'system' time of the gateway, ISO 8601 'expanded' format
        public string Time { get; set; }

        // GPS latitude of the gateway in degree (float, N is +)
        public double Latitude { get; set; }

        // GPS latitude of the gateway in degree (float, E is +)
        public double Longitude { get; set; }

        // GPS altitude of the gateway in meter RX (integer)
        public double Altitude { get; set; }

        // Number of radio packets received (unsigned integer)
        public int Rxnb { get; set; }

        // Number of radio packets received with a valid PHY CRC
        public int Rxok { get; set; }

        // Number of radio packets forwarded (unsigned integer)
        public int Rxfw { get; set; }

        // Percentage of upstream datagrams that were acknowledged
        public double Ackr { get; set; }

        // Number of downlink datagrams received (unsigned integer)
        public int Dwnb { get; set; }

        // Number of packets emitted(unsigned integer)
        public int Txnb { get; set; }

        public string pfrm { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }
    }
}
