namespace LoraWAN_Pipeline.Notifications.GatewayStatusUpdate
{
    using Newtonsoft.Json.Linq;

    public class GatewayStatusMapper
    {
        public GatewayStatusUpdate Map(JObject jsonObj)
        {
            return new GatewayStatusUpdate
            {
                Latitude = double.Parse(jsonObj.First.First["lati"].ToString()),
                Longitude = double.Parse(jsonObj.First.First["long"].ToString()),
                Ackr = double.Parse(jsonObj.First.First["ackr"].ToString()),
                Altitude = double.Parse(jsonObj.First.First["alti"].ToString()),
                Description = jsonObj.First.First["desc"].ToString(),
                Dwnb = int.Parse(jsonObj.First.First["dwnb"].ToString()),
                Email = jsonObj.First.First["mail"].ToString(),
                pfrm = jsonObj.First.First["pfrm"].ToString(),
                Rxfw = int.Parse(jsonObj.First.First["rxfw"].ToString()),
                Rxnb = int.Parse(jsonObj.First.First["rxnb"].ToString()),
                Rxok = int.Parse(jsonObj.First.First["rxok"].ToString()),
                Time = jsonObj.First.First["time"].ToString(),
                Txnb = int.Parse(jsonObj.First.First["txnb"].ToString())
            };
        }
    }
}
