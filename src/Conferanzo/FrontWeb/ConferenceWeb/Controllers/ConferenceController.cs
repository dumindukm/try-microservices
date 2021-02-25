using ConferenceWeb.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConferenceWeb.Controllers
{
    [Authorize]
    public class ConferenceController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ConferenceController(IHttpClientFactory clientFactory)
        {
            this.httpClientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<ConferenceViewModel> model = new List<ConferenceViewModel>();
            var apiClient = httpClientFactory.CreateClient("api_gateway");
            apiClient.SetBearerToken(await HttpContext.GetTokenAsync("access_token"));

            var response = await apiClient.GetAsync("conference");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<IEnumerable<ConferenceViewModel>>(content);
            }
            return View(model);
        }
    }
}
