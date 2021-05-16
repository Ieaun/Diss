namespace LoRaSimulator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using LoRaSimulator.Tcp;
    using LoRaSimulator.Models;
    using Newtonsoft.Json;

    [ApiController]
    [Route("[controller]")]
    public class LoraSimulatorController : ControllerBase
    {
        private readonly ILogger<LoraSimulatorController> _logger;
        private readonly IUdpHandler _udpHandler;

        public LoraSimulatorController(ILogger<LoraSimulatorController> logger, IUdpHandler handler)
        {
            _logger = logger;
            _udpHandler = handler;
        }

        [HttpPost]
        public void Send(string data)
        {
            _udpHandler.Send(data);
        }

        [HttpPost]
        [Route("Udp/Uplink/GatewayStatusUpdate")]
        public void SendUplinkGatewayStatusUpdate(GatewayStatusUpdate gatewayStatusUpdate)
        {
            var serializedPacket = JsonConvert.SerializeObject(gatewayStatusUpdate);
            _udpHandler.Send("{stat:" + serializedPacket + "}");
        }

        [HttpPost]
        [Route("Udp/Uplink/GatewayStatusUpdate/Sample")]
        public void SendUplinkGatewayStatusUpdateSample()
        {
            var sample = System.IO.File.ReadAllText("stat.txt");
            _udpHandler.Send(sample);
        }

        [HttpPost]
        [Route("Udp/Uplink/RecievedMessage")]
        public void SendUplinkRecievedMessage(ReceivedPacket receivedPacket)
        {
            var serializedPacket = JsonConvert.SerializeObject(receivedPacket);
            _udpHandler.Send("{rxpk:"+serializedPacket+"}");
        }

        [HttpPost]
        [Route("Udp/Uplink/RecievedMessage/Sample")]
        public void SendUplinkRecievedMessageSample()
        {
            var sample = System.IO.File.ReadAllText("rxpk.txt");
            _udpHandler.Send(sample);
        }
    }
}
