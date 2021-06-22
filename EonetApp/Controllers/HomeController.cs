using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Query;

namespace EonetApp.Controllers
{
    using Models;
    using Services;

    [Route("api/v1/eonet")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IEonetService _eonetService;

        public HomeController(IEonetService eonetService)
        {
            _eonetService = eonetService;
        }

        /// <summary>
        /// Get all events from EONET API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(List<Event>), StatusCodes.Status200OK)]
        [Route("all")]
        [EnableQuery()]
        public async Task<IEnumerable<Event>> GetAll()
        {
            return await _eonetService.GetAll();
        }

        /// <summary>
        /// Get event details from EONET API
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [Route("details")]
        public async Task<IActionResult> GetById([FromQuery]string id)
        {
            return Ok(await _eonetService.GetById(id));
        }
    }
}
