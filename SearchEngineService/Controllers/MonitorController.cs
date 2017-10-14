using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SearchEngineService.Controllers
{
	[Route("api/ping")]
	public class MonitorController : Controller
	{
		// ensure service is up
		[HttpGet]
		public string Get()
		{
			return "success";
		}
	}
}
