using System;
using Nest;

namespace SearchEngineService.Factories
{
    public class ESConnectionFactory
    {
		private static readonly ESConnectionFactory instance = new ESConnectionFactory();
        private ElasticClient elasticFactory;

		public ElasticClient ElasticFactory { get => elasticFactory; }

		//defaults in args
		private ESConnectionFactory(string host = "127.0.0.1", int port = 9200, string index = "nerds")
		{
            UriBuilder uri = new UriBuilder();
            uri.Host = host;
            uri.Port = port;
            var settings = new ConnectionSettings(uri.Uri).DefaultIndex(index);
            elasticFactory = new ElasticClient(settings);
		}

		public static ESConnectionFactory getInstance()
		{
			return instance;
		}
    }
}
