using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiGatewayService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            httpClient = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            httpClient.SetBearerToken(await _httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            return httpClient;
        }
    }
}
