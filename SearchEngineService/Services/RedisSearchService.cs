using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SearchEngineService.Factories;
using StackExchange.Redis;
using SearchEngineService.Models;
using Newtonsoft.Json;
using System.Linq;

namespace SearchEngineService.Services
{
    public class RedisSearchService
    {
        private readonly IDatabase redisContext;
        private readonly int cacheValidityInMins = 2;

        public RedisSearchService()
        {
            redisContext = RedisConnectionFactory.getInstance().RedisFactory.GetDatabase();
        }

        //send request to queue and wait for results
        public IReadOnlyCollection<Nerd> GetSearchResults(string query)
        {
            var searchResponse = redisContext.StringGet(query);
            if(searchResponse.HasValue)
            {
                var results = JsonConvert.DeserializeObject<List<Nerd>>(searchResponse);
                return results;
            }
            else
            {
                return Enumerable.Empty<Nerd>().ToList();
            }
        }

        public async Task<IReadOnlyCollection<Nerd>> GetSearchResultsAsync(string query)
        {
            var searchResponse = await redisContext.StringGetAsync(query);
            if (searchResponse.HasValue)
            {
                var results = JsonConvert.DeserializeObject<List<Nerd>>(searchResponse);
                return results;
            }
            else
            {
                return Enumerable.Empty<Nerd>().ToList();
            }
        }

        public void CacheResults(string key, string val)
        {
            DateTime dtValidity = DateTime.UtcNow.AddMinutes(cacheValidityInMins);
            var results = redisContext.StringSet(key, val, dtValidity.Subtract(DateTime.UtcNow));
        }

        public async void CacheResultsAsync(string key, string val)
        {
            DateTime dtValidity = DateTime.UtcNow.AddMinutes(cacheValidityInMins);
            await redisContext.StringSetAsync(key, val, dtValidity.Subtract(DateTime.UtcNow));
        }
    }
}
