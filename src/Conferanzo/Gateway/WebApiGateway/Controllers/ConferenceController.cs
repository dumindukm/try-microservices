using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiGateway.Models.Conference;

namespace WebApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ConferenceController : ControllerBase
    {
        HttpClient httpclient;
        ServiceMeta meta;
        public ConferenceController(IHttpClientFactory clientFactory, IOptionsMonitor<ServiceMeta> config)
        {
            httpclient = clientFactory.CreateClient();
            meta = config.CurrentValue;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var result = await httpclient.GetAsync(meta.Conference.Url+"conference");
            var r = await result.Content.ReadAsStringAsync();
            var resultContent = await result.Content.ReadAsStreamAsync();
            var conferences = await JsonSerializer.DeserializeAsync<List<Conference>>(resultContent);

            return new JsonResult(conferences);
        }
    }
}
