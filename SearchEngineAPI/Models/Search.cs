using System;

namespace SearchEngineAPI.Models
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
