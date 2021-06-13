using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EonetApp.Models;
using EonetApp.Services;

namespace EonetApp.Controllers
{
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
        [ProducesResponseType(typeof(EventList), StatusCodes.Status200OK)]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _eonetService.GetAll());
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
