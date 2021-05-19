namespace LoraWAN_Pipeline.ActivationByPersonalization.Decoders
{
    using container.Database;
    using Microsoft.Extensions.Logging;
    using LoraWAN_Pipeline.Queue;
    using System.Threading.Tasks;
    using LoraWAN_Pipeline.Models;
    using System;
    using LoraWAN_Pipeline.Models.LoRaWanPacketSections;
    using System.Linq;
    using LoraWAN_Pipeline.Notifications.ReceivedPackets;
    using LoraWAN_Pipeline.Notifications.TransmitPackets;

    public class LoRaAbpDecoder
    {
        private readonly ILogger<LoRaAbpDecoder> _logger;
        private readonly IQueue _queue;
        private readonly IDatabase _database;

        public LoRaAbpDecoder(ILogger<LoRaAbpDecoder> logger, IQueue queue, IDatabase database)
        {
            this._logger = logger;
            this._queue = queue;
            this._database = database;
        }

        public async Task<bool> IsRegistedDevice(RecievedPacketMetadata receivePacket)
        {
            var decodedMessage = DecodeMessage(receivePacket.Data);

            // check mif it matches any device addresses we have 
            var device = _database.Get(decodedMessage.PhysicalPayload.MacPayload.FrameHeader.DeviceAddress);

            if (device != null)
            {
                await this._queue.EnqueueToStorage( new NewPacket 
                { 
                    PacketType = "Uplink",
                    Uplink = new ReceivedPacket { 
                        decodedPacket = decodedMessage,
                        isRegesteredDevice = true,
                        metadata = receivePacket
                    }
                });
                return true;
            }
            return false;
        }

        public DecodedLoraPacket DecodeMessage(string encodedPhysicalPayload)
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
                MacHeader = hexValuesList[0],
                MIC = string.Join("",hexValuesList).Substring((hexValuesList.Count * 2) -8, 8)
            };
            hexValuesList.RemoveRange(hexValuesList.Count -4, 4);
            hexValuesList.RemoveAt(0);

            var frameHeader = new FrameHeader 
            {       
                DeviceAddress = hexValuesList[3] + hexValuesList[2] + hexValuesList[1] + hexValuesList[0], // big endian
                FrameControlOctet = hexValuesList[4],
                FrameCounter = hexValuesList[6] + hexValuesList[5] // big endian        
            };

            // frame options is optional, need to work out size / if its present using octet
            var binaryFrameControl = Convert.ToString(Convert.ToInt32(frameHeader.FrameControlOctet, 16), 2);         
            var frameOptionsOffset = Convert.ToInt32(binaryFrameControl.Substring(binaryFrameControl.Length - 3, 3));
            if (frameOptionsOffset != 0) {
                for (int i = 1; i <= frameOptionsOffset; i++)
                {
                    frameHeader.FrameOptions += hexValuesList[6 + i];
                }
            }

            // FramePayload
            var values = string.Join("", hexValuesList);
            var payloadOffset = 16 + (frameOptionsOffset * 2);

            var macPayload = new MacPayload
            {
                FramePort = hexValuesList[7 + frameOptionsOffset],
                FramePayload = values.Substring(payloadOffset, values.Length - payloadOffset),
                FrameHeader = frameHeader
            };        

            // combine structures
            physicalPayload.MacPayload = macPayload;
            decodedPacket.PhysicalPayload = physicalPayload;

            return decodedPacket;
        }

    }
}
