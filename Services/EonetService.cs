using System;
using System.Collections.Generic;
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
        private readonly MemoryCacheEntryOptions _cachingOptions;

        const string cacheEntityKey = "eonet_list";

        public EonetService(IEonetTrackerClient eonetTrackerClient, IMemoryCache memoryCache)
        {
            _eonetTrackerClient = eonetTrackerClient;
            _memoryCache = memoryCache;

            _cachingOptions = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal,
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            return await GetList();
        }

        public async Task<Event> GetById(string id)
        {
            var list = await GetList();

            return list.SingleOrDefault(e => e.Id == id);
        }

        private async Task<IEnumerable<Event>> GetList()
        {
            if (!_memoryCache.TryGetValue(cacheEntityKey, out IEnumerable<Event> eventList))
            {
                var list = await _eonetTrackerClient.GetAllEvents();

                eventList = list.Events;

                _memoryCache.Set(cacheEntityKey, eventList);
            }

            return eventList;
        }
    }
}
