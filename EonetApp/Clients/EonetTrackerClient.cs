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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UrlsConfiguration _configuration;

        public EonetTrackerClient(IHttpClientFactory httpClientFactory, IOptions<UrlsConfiguration> configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration.Value;
        }

        public async Task<EventList> GetAllEvents()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(_configuration.EonetUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<EventList>();
        }
    }
}
