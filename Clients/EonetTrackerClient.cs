using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace EonetApp.Clients
{
    using Models;
    using Configuration;

    public class EonetTrackerClient : IEonetTrackerClient
    {
        private readonly UrlsConfiguration _configuration;

        public EonetTrackerClient(IOptions<UrlsConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public async Task<EventList> GetAllEvents()
        {
            var requestUri = new Uri(_configuration.EonetUrl);
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<EventList>();
        }
    }
}
