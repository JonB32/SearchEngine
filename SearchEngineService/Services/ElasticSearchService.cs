using System;
using SearchEngineService.Publishers;
using SearchEngineService.Factories;
using SearchEngineService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace SearchEngineService.Services
{
    public class ElasticSearchService : IDataSourceService
    {
        private ElasticClient esContext;

        //communicates with ElasticSearch API via message queue
        public ElasticSearchService()
        {
            esContext = ESConnectionFactory.getInstance().ElasticFactory;
        }

        //send request to queue and wait for results
        public IReadOnlyCollection<Nerd> GetSearchResults(string query)
        {
            var searchResponse = esContext.Search<Nerd>(s => s
                                                          .Type("nerds")
                                                          .From(0)
                                                          .Size(10)
                                                          .Query(q => q
                                                                 .Match(m => m
                                                                        .Field(f => f.Name)
                                                                        .Query(query))));

            var nerds = searchResponse.Documents;
            return nerds;
        }

		public async Task<IReadOnlyCollection<Nerd>> GetSearchResultsAsync(string query)
		{
			var searchResponse = await esContext.SearchAsync<Nerd>(s => s
															  .Type("nerds")
															  .From(0)
															  .Size(10)
															  .Query(q => q
																	 .Match(m => m
																			.Field(f => f.Name)
																			.Query(query))));

			var nerds = searchResponse.Documents;
			return nerds;
        }
    }
}
