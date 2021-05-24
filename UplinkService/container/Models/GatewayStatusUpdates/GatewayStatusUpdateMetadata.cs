namespace UplinkService.Models.GatewayStatusUpdates
{
    using Newtonsoft.Json;

    public class GatewayStatusUpdateMetadata 
    {
        // Descriptions taken from: https://github.com/Lora-net/packet_forwarder/blob/master/PROTOCOL.TXT

        // UTC 'system' time of the gateway, ISO 8601 'expanded' format
        [JsonProperty("time")]
        public string Time { get; set; }

        // GPS latitude of the gateway in degree (float, N is +)
        [JsonProperty("lati")]
        public double Latitude { get; set; }

        // GPS latitude of the gateway in degree (float, E is +)
        [JsonProperty("long")]
        public double Longitude { get; set; }

        // GPS altitude of the gateway in meter RX (integer)
        [JsonProperty("alti")]
        public double Altitude { get; set; }

        // Number of radio packets received (unsigned integer)
        [JsonProperty("rxnb")]
        public int Rxnb { get; set; }

        // Number of radio packets received with a valid PHY CRC
        [JsonProperty("rxok")]
        public int Rxok { get; set; }

        // Number of radio packets forwarded (unsigned integer)
        [JsonProperty("rxfw")]
        public int Rxfw { get; set; }

        // Percentage of upstream datagrams that were acknowledged
        [JsonProperty("ackr")]
        public double Ackr { get; set; }

        // Number of downlink datagrams received (unsigned integer)
        [JsonProperty("dwnb")]
        public int Dwnb { get; set; }

        // Number of packets emitted(unsigned integer)
        [JsonProperty("txnb")]
        public int Txnb { get; set; }

        [JsonProperty("pfrm")]
        public string pfrm { get; set; }

        [JsonProperty("mail")]
        public string Email { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }
    }
}
