namespace UplinkService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UplinkService.Database;
    using System;

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
        [Route("/Create/")]
        public async Task<ActionResult> Create(StubObject packet)
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
        public async Task<ActionResult<StubObject>> Get(int id)
        {
            var foundObject = await _database.Get(id);
            return foundObject == null ? NotFound(): Ok(foundObject);
        }

        
        [HttpGet]
        [Route("/GetAll")]
        public async Task<ActionResult<List<StubObject>>> GetAll()
        {
            var stubObjectsList = await _database.GetAll();
            return stubObjectsList.Count == 0 ? NotFound() : Ok(stubObjectsList);
        }

        [HttpPost]
        [Route("/Update/")]
        public async Task<ActionResult> Update(StubObject packet)
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
        [Route("/Delete/")]
        public async Task<ActionResult> Delete(StubObject packet)
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
