using System;
using System.Collections.Generic;
using SearchEngineService.Models;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SearchEngineService.Services
{
    public class DataSourceFactory : IDataSourceService
    {
        public DataSourceFactory()
        {
        }

        public IReadOnlyCollection<Nerd> GetSearchResults(string query)
        {
            //Try to get data in following order, should do asynchornous where 
            // 1st return wins
            // 1: cache
            // 2: Elastic Search
            // 3: MongoDB
            IReadOnlyCollection<Nerd> results = new List<Nerd>();
            string serializedResults;

            RedisSearchService redisSearch = new RedisSearchService();
            results = redisSearch.GetSearchResults(query);
			if (results.Count > 0)
			{
				return results;
			}

            ElasticSearchService elasticSearch = new ElasticSearchService();
            results = elasticSearch.GetSearchResults(query);
            if(results.Count > 0)
            {
                serializedResults = JsonConvert.SerializeObject(results);
                redisSearch.CacheResults(query, serializedResults);
                return results;
            }

            MongoDBSearchService mongoSearch = new MongoDBSearchService();
			results = mongoSearch.GetSearchResults(query);
			if (results.Count > 0)
			{
				serializedResults = JsonConvert.SerializeObject(results);
				redisSearch.CacheResults(query, serializedResults);
				return results;
			}

			return results;
        }

        public async Task<IReadOnlyCollection<Nerd>> GetSearchResultsAsync(string query)
        {
            //Try to get data in following order, should do asynchornous where 
            // 1st return wins
            // 1: cache
            // 2: Elastic Search
            // 3: MongoDB
            var results = new List<Task<IReadOnlyCollection<Nerd>>>();
            IReadOnlyCollection<Nerd> res = new List<Nerd>();
            string serializedResults;

			//Let's cancel remaining tasks after we have one complete with results
			//CancellationTokenSource cts = new CancellationTokenSource();

			RedisSearchService redisSearch = new RedisSearchService();
            results.Add(redisSearch.GetSearchResultsAsync(query));

			ElasticSearchService elasticSearch = new ElasticSearchService();
            results.Add(elasticSearch.GetSearchResultsAsync(query));

            MongoDBSearchService mongoSearch = new MongoDBSearchService();
            results.Add(mongoSearch.GetSearchResultsAsync(query));

            while(results.Count > 0)
            {
                var result = await Task.WhenAny(results);
                results.Remove(result);

                res = await result;
                if(res.Count > 0)
                {
					serializedResults = JsonConvert.SerializeObject(res);
					redisSearch.CacheResultsAsync(query, serializedResults);
                    return res;
                }
            }

            return res;
            //return await Task.WhenAny(results).Results;
		}
    }
}
