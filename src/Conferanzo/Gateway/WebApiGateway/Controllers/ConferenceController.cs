using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGateway.Models.Conference;

namespace WebApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ConferenceController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<Conference> conferences = new List<Conference>();
            return new JsonResult(conferences);
        }
    }
}
