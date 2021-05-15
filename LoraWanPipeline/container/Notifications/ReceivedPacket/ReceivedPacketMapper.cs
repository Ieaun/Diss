namespace LoraWAN_Pipeline.Notifications.ReceivedPacket
{
    using Newtonsoft.Json.Linq;

    public class ReceivedPacketMapper
    {
        public ReceivedPacket Map(JObject jsonObj)
        {
            return new ReceivedPacket
            {
                metadata = new ReceivedPacketMetadata
                {
                    Timestamp = int.Parse(jsonObj.First.First["tmst"].ToString()),
                    Frequency = double.Parse(jsonObj.First.First["freq"].ToString()),
                    Channel = int.Parse(jsonObj.First.First["chan"].ToString()),
                    RadioFrequencyChannel = int.Parse(jsonObj.First.First["rfch"].ToString()),
                    Stat = int.Parse(jsonObj.First.First["stat"].ToString()),
                    Modulation = int.Parse(jsonObj.First.First["modu"].ToString()),
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
