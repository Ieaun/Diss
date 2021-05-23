namespace LoraWAN_Pipeline.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using LoraWAN_Pipeline.Tcp;
    using container.Database;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;

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
        [Route("Pipeline/Create/")]
        public async Task<ActionResult> Create(RegisteredDevice device)
        {
            try
            {
                await _database.Create(device);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to create object {@Object} with error {@Exception}", device, e.Message);
                return Conflict();
            }
        }

        [HttpGet]
        [Route("Pipeline/Get/")]
        public async Task<ActionResult<RegisteredDevice>> Get(int Id)
        {
            var foundDevice = await _database.Get(Id);
            return foundDevice == null ? NotFound() : Ok(foundDevice);
        }

        [HttpGet]
        [Route("Pipeline/GetByAddress/")]
        public async Task<ActionResult<RegisteredDevice>> Get(string deviceAddress)
        {
            var foundDevice = await _database.Get(deviceAddress);
            return foundDevice == null ? NotFound() : Ok(foundDevice);
        }


        [HttpGet]
        [Route("Pipeline/GetAll")]
        public async Task<ActionResult<List<RegisteredDevice>>> GetAll()
        {
            var registeredDevices = await _database.GetAll();
            return registeredDevices.Count == 0 ? NotFound() : Ok(registeredDevices);
        }

        [HttpPost]
        [Route("Pipeline/Update/")]
        public async Task<ActionResult> Update(RegisteredDevice device)
        {
            try
            {
                await _database.Update(device);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to update object {@Object} with error {@Exception}", device, e.Message);
                return Conflict();
            }
        }

        [HttpPost]
        [Route("Pipeline/Delete/")]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                await _database.Delete(Id);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to delete object with Id {@Id} with error {@Exception}", Id, e.Message);
                return Conflict();
            }
        }
    }
}
