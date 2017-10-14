using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchEngineAPI.Services;
using SearchEngineAPI.Models;

namespace SearchEngineAPI.Controllers
{
    [Route("api/search")]
    public class SearchController : Controller
    {
        // GET api/search
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/search/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Search searchRes = new Search(){ Query = "value" };
            return new OkObjectResult(searchRes);
        }

        // POST api/search
        [HttpPost]
        public IActionResult Post([FromBody]Search query)
        {
			if (query == null)
				return BadRequest();
            SearchService searchService = new SearchService();

            return new OkObjectResult(searchService.GetSearchResults(query.Query));
        }

        // PUT api/search/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/search/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
