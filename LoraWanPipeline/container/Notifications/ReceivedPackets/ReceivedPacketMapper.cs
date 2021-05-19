namespace LoraWAN_Pipeline.Notifications.ReceivedPackets
{
    using Newtonsoft.Json.Linq;

    public class ReceivedPacketMapper
    {
        public ReceivedPacket Map(JObject jsonObj)
        {
            var Timestamp = int.Parse(jsonObj.First.First["tmst"].ToString());
            var Frequency = double.Parse(jsonObj.First.First["freq"].ToString());
            var Channel = int.Parse(jsonObj.First.First["chan"].ToString());
            var RadioFrequencyChannel = int.Parse(jsonObj.First.First["rfch"].ToString());
            var Stat = int.Parse(jsonObj.First.First["stat"].ToString());
            var Modulation = int.Parse(jsonObj.First.First["modu"].ToString());
            var DataRate = jsonObj.First.First["datr"].ToString();
            var CodingRate = jsonObj.First.First["codr"].ToString();
            var RecievedSignalStrenghtIndicator = int.Parse(jsonObj.First.First["rssi"].ToString());
            var LuminanceSignalToRatio = double.Parse(jsonObj.First.First["lsnr"].ToString());
            var Size = int.Parse(jsonObj.First.First["size"].ToString());
            var Data = jsonObj.First.First["data"].ToString();



            return new ReceivedPacket
            {
                metadata = new TransmitPacketMetadata
                {
                    Timestamp = int.Parse(jsonObj.First.First["tmst"].ToString()),
                    Frequency = double.Parse(jsonObj.First.First["freq"].ToString()),
                    Channel = int.Parse(jsonObj.First.First["chan"].ToString()),
                    RadioFrequencyChannel = int.Parse(jsonObj.First.First["rfch"].ToString()),
                    Stat = int.Parse(jsonObj.First.First["stat"].ToString()),
                    Modulation = jsonObj.First.First["modu"].ToString(),
                    DataRate = jsonObj.First.First["datr"].ToString(),
                    CodingRate = jsonObj.First.First["codr"].ToString(),                                                               
                    RecievedSignalStrenghtIndicator = int.Parse(jsonObj.First.First["rssi"].ToString()),
                    LuminanceSignalToRatio = double.Parse(jsonObj.First.First["lsnr"].ToString()),
                    Size = int.Parse(jsonObj.First.First["size"].ToString()),
                    Data = jsonObj.First.First["data"].ToString()
                }
            };
        }
    }
}
