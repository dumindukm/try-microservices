using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConferenceWeb.Services
{
    public class ApiGatewayService
    {
        private HttpClient httpClient;
        public ApiGatewayService(HttpClient client)
        {
            httpClient = client;

        }

        public HttpClient GetHttpClient()
        {
            //httpClient.SetBearerToken(await HttpContext.GetTokenAsync("access_token"));
            return httpClient;
        }
    }
}
