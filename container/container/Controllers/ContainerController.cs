namespace LoraWAN_Pipeline.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using LoraWAN_Pipeline.Tcp;

    [ApiController]
    [Route("api/[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly ILogger<ContainerController> _logger;
        private readonly ITcpHandler _tcpHandler;

        public ContainerController(ILogger<ContainerController> logger,ITcpHandler tcpHandler)
        {
            this._logger = logger;
            this._tcpHandler = tcpHandler;
        }

        [HttpGet]
        public void Get(string packet)
        {
            
        }
    }
}
