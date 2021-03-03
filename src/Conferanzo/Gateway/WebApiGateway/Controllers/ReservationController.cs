using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiGateway.Models.Reservation;

namespace WebApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        HttpClient httpclient;
        ServiceMeta meta;
        public ReservationController(IHttpClientFactory clientFactory, IOptionsMonitor<ServiceMeta> config)
        {
            httpclient = clientFactory.CreateClient();
            meta = config.CurrentValue;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody]Reservation reservation)
        {
            var content = JsonConvert.SerializeObject(reservation);
            var data = new StringContent(content, Encoding.UTF8, "application/json");

            var result = await httpclient.PostAsync(meta.Reservation.Url + "reservation_start", data);
            var resultContent = await result.Content.ReadAsStringAsync();
            //var resultContent = await result.Content.ReadAsStreamAsync();

            return new JsonResult(new { TransactionId = resultContent }); ;
        }
    }
}
