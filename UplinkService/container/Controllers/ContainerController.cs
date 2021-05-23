namespace UplinkService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UplinkService.Database;
    using System;
    using UplinkService.Models.Servers;

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
        [Route("Uplink/Create/")]
        public async Task<ActionResult> Create(Server packet)
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
        [Route("Uplink/Get/")]
        public async Task<ActionResult<Server>> Get(int id)
        {
            var foundObject = await _database.Get(id);
            return foundObject == null ? NotFound(): Ok(foundObject);
        }

        
        [HttpGet]
        [Route("Uplink/GetAll")]
        public async Task<ActionResult<List<Server>>> GetAll()
        {
            var serverList = await _database.GetAll();
            return serverList.Count == 0 ? NotFound() : Ok(serverList);
        }

        [HttpPost]
        [Route("Uplink/Update/")]
        public async Task<ActionResult> Update(Server packet)
        {
            try
            {
                await _database.Update(packet);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to update object {@Object} with error {@Exception}", packet, e.Message);
                return Conflict();
            }
        }

        [HttpPost]
        [Route("Uplink/Delete/")]
        public async Task<ActionResult> Delete(Server packet)
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
