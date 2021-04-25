namespace LoRaSimulator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using LoRaSimulator.Tcp;

    [ApiController]
    [Route("[controller]")]
    public class LoraSimulatorController : ControllerBase
    {
        private readonly ILogger<LoraSimulatorController> _logger;
        private readonly ITcpHandler _handler;

        public LoraSimulatorController(ILogger<LoraSimulatorController> logger, ITcpHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        [HttpPost]
        public void Send(string data)
        {
            LoraPacket packet = new LoraPacket
            {
                Payload = data
            };
            _handler.Send(packet);
        }

        [HttpPost]
        [Route("Uplink")]
        public void SendUplink(string data)
        {
            LoraPacket packet = new LoraPacket
            {
                Payload = data
            };
            _handler.Send(packet);
        }

        [HttpPost]
        [Route("Downlink")]
        public void SendDownlink(string data)
        {
            LoraPacket packet = new LoraPacket
            {
                Payload = data
            };
            _handler.Send(packet);
        }
    }
}
