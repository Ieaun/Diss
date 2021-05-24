namespace StorageService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using StorageService.Database;
    using System;
    using StorageService.Notifications.ReceivedPackets;

    [ApiController]
    [Route("api/[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly ILogger<ContainerController> _logger;
        private readonly IDatabase _database;

        public ContainerController(ILogger<ContainerController> logger, IDatabase database)
        {
            this._logger = logger;
            this._database = database;
        }

        [HttpPost]
        [Route("Storage/Create/")]
        public async Task<ActionResult> Create(NewPacket packet)
        {
            try
            {
                await _database.Create(packet);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to create object {@Object} with error {@Exception}", packet, e.Message);
                return Conflict();
            }
        }

        [HttpGet]
        [Route("Storage/Get/")]
        public async Task<ActionResult<NewPacket>> Get(string deviceAddress)
        {
            var foundObject = await _database.Get(deviceAddress);
            return foundObject == null ? NotFound(): Ok(foundObject);
        }

        
        [HttpGet]
        [Route("Storage/GetAll")]
        public async Task<ActionResult<List<NewPacket>>> GetAll()
        {
            var packetsList = await _database.GetAll();
            return packetsList.Count == 0 ? NotFound() : Ok(packetsList);
        }

        [HttpPost]
        [Route("Storage/Delete/")]
        public async Task<ActionResult> Delete(NewPacket packet)
        {
            try
            {
                await _database.Delete(packet);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to delete object {@Object} with error {@Exception}", packet, e.Message);
                return Conflict();
            }
        }
    }
}
