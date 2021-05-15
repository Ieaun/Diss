namespace LoraWAN_Pipeline.ActivationByPersonalization.Decoders
{
    using container.Database;
    using LoraWAN_Pipeline.Notifications.ReceivedPacket;
    using Microsoft.Extensions.Logging;
    using LoraWAN_Pipeline.Queue;
    using System.Threading.Tasks;
    using LoraWAN_Pipeline.Decoders.ActivationByPersonalization;
    using LoraWAN_Pipeline.Models;
    using System;
    using LoraWAN_Pipeline.Models.LoRaWanPacketSections;
    using System.Linq;

    public class LoRaAbpDecoder
    {
        private readonly ILogger<LoRaAbpDecoder> _logger;
        private readonly IQueue _queue;
        private readonly IDatabase _database;
        private readonly AbpValidator _abpValidator;

        public LoRaAbpDecoder(ILogger<LoRaAbpDecoder> logger, IQueue queue, IDatabase database, AbpValidator abpValidator)
        {
            this._logger = logger;
            this._queue = queue;
            this._database = database;
            this._abpValidator = abpValidator;
        }

        public async Task<string> DecodeMessage(ReceivedPacket receivePacket) 
        {
            string decodedMessage = null;

            DecodedLoraPacket decodedPacket = DecodePhysicalPayload(receivePacket.metadata.Data);

            //validate the decoded packet
            //this._abpValidator.Validate(decodedPacket);                  

            return decodedMessage;
        }

        public DecodedLoraPacket DecodePhysicalPayload(string encodedPhysicalPayload)
        {
            // conver to hex
            var byteArray = Convert.FromBase64String(encodedPhysicalPayload);
            var hexValuesList = BitConverter.ToString(byteArray).Split("-").ToList();

            // create packet
            DecodedLoraPacket decodedPacket = new DecodedLoraPacket() 
            {
                OriginalHexString = string.Join("", hexValuesList)
            };

            var physicalPayload = new PhysicalPayload
            {
                OriginalHexString = string.Join("", hexValuesList),
                MacHeader = hexValuesList[0],
                MIC = string.Join("",hexValuesList).Substring((hexValuesList.Count * 2) -8, 8)
            };

            // TODO : need sise of the mac payload items ?!?!? 
            hexValuesList.RemoveRange((hexValuesList.Count * 2)-5, 4);
            hexValuesList.RemoveAt(0);
            var macPayload = new MacPayload
            {
                OriginalHexString = string.Join("", hexValuesList),            
            };

            var frameHeader = new FrameHeader 
            {       
                OriginalHexString = string.Join("", hexValuesList),
                DeviceAddress = string.Join("", hexValuesList).Substring(0, 4),
                FrameControlOctet = hexValuesList[4],
                FrameCounter = string.Join("", hexValuesList).Substring(5, 2)          
            };
            frameHeader.FrameOptions = frameHeader.FrameControlOctet[1] != '0' ?
                string.Join("", hexValuesList).Substring(7, (frameHeader.FrameControlOctet[1] - '0') * 2)  : null;

            hexValuesList.RemoveRange(0, 7 + (frameHeader.FrameControlOctet[1] - '0'));

            macPayload.FramePort = hexValuesList[0];
            macPayload.FramePayload = hexValuesList.GetRange(1, hexValuesList.Count - 2).ToString();

            // combine structures
            macPayload.FrameHeader = frameHeader;
            physicalPayload.MacPayload = macPayload;
            decodedPacket.PhysicalPayload = physicalPayload;

            return decodedPacket;
        }

        public async Task<bool> IsRegistedDevice(ReceivedPacket receivePacket) 
        {
            var decodedMessage = DecodeMessage(receivePacket);
            if (decodedMessage != null) {
                await this._queue.EnqueueToStorage(receivePacket);
                return true;
            }
            return false;
        }
    }
}
