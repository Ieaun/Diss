namespace UplinkService.Models
{
    public class SemtechUplinkHeaderMetaData
    {
        // [0]
        public byte ProtocolVersion { get; set; }

        // [1-2]
        public byte[] Token { get; set; }

        // [3]
        public byte PushType { get; set; }

        // [4-13]
        public byte[] GatewayEUI { get; set; }
    }
}
