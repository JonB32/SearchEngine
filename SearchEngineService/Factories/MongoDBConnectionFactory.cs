using System;
using MongoDB.Driver;
using System.Text;

namespace SearchEngineService.Factories
{
    public class MongoDBConnectionFactory
    {
		private static readonly MongoDBConnectionFactory instance = new MongoDBConnectionFactory();
        private MongoClient mongoDBFactory;


		public MongoClient MongoDBFactory { get => mongoDBFactory; }

		//defaults in args
		private MongoDBConnectionFactory(string host = "127.0.0.1", int port = 27017, string db = "nerds")
		{
            StringBuilder url = new StringBuilder();
            url.Append("mongodb://");
            url.Append(host);
            url.Append(":" + port);
            url.Append("/" + db);
            url.Append("?connect=replicaSet");

            mongoDBFactory = new MongoClient(url.ToString());
		}

		public static MongoDBConnectionFactory getInstance()
		{
			return instance;
		}
    }
}
