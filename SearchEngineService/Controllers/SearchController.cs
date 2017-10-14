using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchEngineService.Models;
using SearchEngineService.Services;

namespace SearchEngineService.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Search query)
        {
            if (query == null)
                return BadRequest();

            IDataSourceService searchEngine = new DataSourceFactory();
            IReadOnlyCollection<Nerd> res = searchEngine.GetSearchResults(query.Query);

            return new OkObjectResult(res);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
