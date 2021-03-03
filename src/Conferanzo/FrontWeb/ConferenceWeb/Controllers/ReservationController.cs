using ConferenceWeb.Models;
using ConferenceWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceWeb.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApiGatewayService gatewayService;
        public ReservationController(ApiGatewayService service)
        {
            this.gatewayService = service;
        }
        public async Task<IActionResult> Index()
        {
            var apiClient = await gatewayService.GetHttpClient();

            var reservation = new ReservationViewModel();
            var content = JsonConvert.SerializeObject(reservation);
            var data = new StringContent(content, Encoding.UTF8, "application/json");


            var response = await apiClient.PostAsync("reservation",data);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                //model = JsonConvert.DeserializeObject<IEnumerable<ConferenceViewModel>>(content);
            }

            return View();
        }
    }
}
