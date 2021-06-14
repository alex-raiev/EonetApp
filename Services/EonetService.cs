using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace EonetApp.Services
{
    using Clients;
    using Models;

    public class EonetService : IEonetService
    {
        private readonly IEonetTrackerClient _eonetTrackerClient;
        private readonly IMemoryCache _memoryCache;

        public EonetService(IEonetTrackerClient eonetTrackerClient, IMemoryCache memoryCache)
        {
            _eonetTrackerClient = eonetTrackerClient;
            _memoryCache = memoryCache;
        }

        public async Task<EventList> GetAll()
        {
            return await _eonetTrackerClient.GetAllEvents();
        }

        public async Task<Event> GetById(string id)
        {
            if (!_memoryCache.TryGetValue(id, out Event @event))
            {
                var events = await _eonetTrackerClient.GetAllEvents();

                var cachingOptions = new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal,
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(1),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };

                if (@event == null)
                {
                    @event = events.Events.SingleOrDefault(x => x.Id == id);
                }
                
                _memoryCache.Set(id, @event);
            }

            return @event;
        }
    }
}
