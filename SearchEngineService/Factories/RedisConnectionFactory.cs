using System;
using StackExchange.Redis;

namespace SearchEngineService.Factories
{
    public class RedisConnectionFactory
    {
		private static readonly RedisConnectionFactory instance = new RedisConnectionFactory();
		private ConnectionMultiplexer redisFactory;

		public ConnectionMultiplexer RedisFactory { get => redisFactory; }

		//defaults in args
		private RedisConnectionFactory(string host = "localhost", int port = 6379)
		{
            redisFactory = ConnectionMultiplexer.Connect(host);
		}

		public static RedisConnectionFactory getInstance()
		{
			return instance;
		}
    }
}
