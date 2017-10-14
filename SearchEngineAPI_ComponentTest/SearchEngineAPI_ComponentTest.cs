using System;
using Xunit;
using RA;

namespace SearchEngineAPI_ComponentTest
{
    public class SearchEngineAPI_ComponentTest
    {
        readonly static string SearchAPIBaseURL = "http://localhost:5000/api";
        readonly static string SearchAPIURL = SearchAPIBaseURL + "/Search";
        readonly static string MonitorAPIURL = SearchAPIBaseURL + "/ping";

        [Fact]
        public void Search_GetAPI_Succeed()
        {
            //restassured testing
            new RestAssured()
                .Given()
                    .Header("Content-Type", "application/json; charset=utf-8")
                .When()
                    .Get(SearchAPIURL + "/1")
                .Then()
                    .TestBody("GETAPI_Test", x => x.Query == "value")
                    .Assert("GETAPI_Test");
        }

		[Fact]
		public void Search_PingAPI_Succeed()
		{
			//restassured testing
			new RestAssured()
				.Given()
					.Header("Content-Type", "application/json; charset=utf-8")
				.When()
					.Get(MonitorAPIURL)
				.Then()
                    .TestBody("PingAPI_Test", x => x.Result == "success")
					.Assert("PingAPI_Test");
		}
    }
}
