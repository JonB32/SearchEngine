using System;
using Xunit;
using SearchEngineAPI.Controllers;
using SearchEngineAPI.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssert;

namespace SearchEngineAPI_UnitTests
{
    public class SearchEngineAPI_Controller_UnitTest
    {
        [Fact]
        public void SearchController_Get_Test()
        {
            SearchController controller = new SearchController();
            var result = (ObjectResult)controller.Get(1);
            Search actual = (Search)result.Value;

            Search expected = new Search{ Query = "value" };

            //assert
            Assert.Equal(expected.Query, actual.Query);
        }

		[Fact]
		public void MonitorController_Ping_Test()
		{
            MonitorController controller = new MonitorController();
			var result = (ObjectResult)controller.Get();
			Ping actual = (Ping)result.Value;

            Ping expected = new Ping { Result = "success" };

			//assert
            Assert.Equal(expected.Result, actual.Result);
		}
    }
}
