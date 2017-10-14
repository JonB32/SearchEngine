using System;
using SearchEngineAPI.Publishers;
using System.Collections.Generic;
using SearchEngineAPI.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SearchEngineAPI.Services
{

    public class SearchService
    {

        public IReadOnlyCollection<Nerd> GetSearchResults(string query)
        {
            IPublishers searchReqPub = new SearchRequestPublisher();
            string res = searchReqPub.GetMessage(query);
            IReadOnlyCollection<Nerd> searchResult = JsonConvert.DeserializeObject<IReadOnlyCollection<Nerd>>(res);
            searchReqPub.CloseConnection();

            return searchResult;
        }

		public async Task<IReadOnlyCollection<Nerd>> GetSearchResultsAsync(string query)
		{
			IPublishers searchReqPub = new SearchRequestPublisher(true);
			string res = await searchReqPub.GetMessageAsync(query);
			IReadOnlyCollection<Nerd> searchResult = JsonConvert.DeserializeObject<IReadOnlyCollection<Nerd>>(res);
			searchReqPub.CloseConnection();

			return searchResult;
		}
    }
}