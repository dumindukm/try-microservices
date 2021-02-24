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
        public ConferenceController()
        {

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<ConferenceViewModel> model = new List<ConferenceViewModel>();
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(await HttpContext.GetTokenAsync("access_token"));

            var response = await apiClient.GetAsync("https://localhost:44387/api/conference");
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
