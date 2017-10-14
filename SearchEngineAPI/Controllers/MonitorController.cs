using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchEngineAPI.Services;
using SearchEngineAPI.Models;

namespace SearchEngineAPI.Controllers
{
	[Route("api/ping")]
	public class MonitorController : Controller
	{
		// ensure service is up
		[HttpGet]
		public IActionResult Get()
		{
            Ping pingRes = new Ping() { Result = "success" };
            return new OkObjectResult(pingRes);
		}
	}
}
