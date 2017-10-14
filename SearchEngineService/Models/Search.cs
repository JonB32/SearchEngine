using System;
namespace SearchEngineService.Models
{
    public class Search
    {
        private string query;

        public Search()
        {
        }

        public string Query { get => query; set => query = value; }
    }
}
