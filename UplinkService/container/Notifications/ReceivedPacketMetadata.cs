namespace UplinkService.Notifications
{
    using Newtonsoft.Json;

    public class ReceivedPacketMetadata 
    {
        // Descriptions taken from: https://github.com/Lora-net/packet_forwarder/blob/master/PROTOCOL.TXT

        // e.g. 238504441
        [JsonProperty("tmst")]
        public double Timestamp { get; set; }

        // e.g. 868.1
        // RX central frequency in MHz (unsigned float, Hz precision)
        [JsonProperty("freq")]
        public double Frequency { get; set; }

        // e.g. 0
        // Concentrator "IF" channel used for RX (unsigned integer)
        [JsonProperty("chan")]
        public int Channel { get; set; }

        // e.g. 0
        //  Concentrator "RF chain" used for RX (unsigned integer)
        [JsonProperty("rfch")]
        public int RadioFrequencyChannel { get; set; }

        // e.g. 1
        // CRC status: 1 = OK, -1 = fail, 0 = no CRC
        [JsonProperty("stat")]
        public int Stat { get; set; }

        // e.g. "LORA"
        // Modulation identifier "LORA" or "FSK"
        [JsonProperty("modu")]
        public string Modulation { get; set; }

        // e.g. "SF7BW125"
        // FSK datarate (unsigned, in bits per second)
        [JsonProperty("datr")]
        public string DataRate { get; set; }

        // e.g. "4/5"
        // LoRa ECC coding rate identifier
        [JsonProperty("codr")]
        public string CodingRate { get; set; }

        // e.g. -32
        // RSSI in dBm (signed integer, 1 dB precision)
        [JsonProperty("rssi")]
        public int RecievedSignalStrenghtIndicator { get; set; }

        // e.g. 9.0
        // Lora SNR ratio in dB (signed float, 0.1 dB precision)
        [JsonProperty("lsnr")]
        public double LuminanceSignalToRatio { get; set; }

        // e.g. 26
        // RF packet payload size in bytes (unsigned integer)
        [JsonProperty("size")]
        public int Size { get; set; }

        // e.g "QCIQASaAAAABz+ZtENUq9UCUIgrynehqYt8=\"
        // Base64 encoded RF packet payload, padded
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
