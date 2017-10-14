using System;
using System.Collections.Generic;
using SearchEngineService.Models;
using System.Threading.Tasks;

namespace SearchEngineService.Services
{
    public interface IDataSourceService
    {
        IReadOnlyCollection<Nerd> GetSearchResults(string query);

        Task<IReadOnlyCollection<Nerd>> GetSearchResultsAsync(string query);
    }
}
