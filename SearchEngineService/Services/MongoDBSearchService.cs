using System;
using SearchEngineService.Factories;
using SearchEngineService.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngineService.Services
{
    public class MongoDBSearchService : IDataSourceService
    {
        private MongoClient mongoContext;

        public MongoDBSearchService()
        {
            mongoContext = MongoDBConnectionFactory.getInstance()
                                                   .MongoDBFactory;
        }

		//send request to queue and wait for results
		public IReadOnlyCollection<Nerd> GetSearchResults(string query)
		{
            var nerdsDatabase = mongoContext.GetDatabase("nerds");
            var nerdsCollection = nerdsDatabase.GetCollection<Nerd>("nerds");

			IReadOnlyCollection<Nerd> searchResponse = nerdsCollection.Find<Nerd>(s => s.Name == query)
																	  .ToList();
			return searchResponse;
		}

		//send request to queue and wait for results
        public async Task<IReadOnlyCollection<Nerd>> GetSearchResultsAsync(string query)
		{
			var nerdsDatabase = mongoContext.GetDatabase("nerds");
			var nerdsCollection = nerdsDatabase.GetCollection<Nerd>("nerds");

			var searchResponse = await nerdsCollection.FindAsync<Nerd>(s => s.Name == query);
            return searchResponse.ToList();
		}
    }
}
