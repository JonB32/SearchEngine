using System;
using RabbitMQ.Client;

namespace SearchEngineService.Factories
{
    public class MQConnectionFactory
    {
        private static readonly MQConnectionFactory instance = new MQConnectionFactory();
        private ConnectionFactory rabbitMQFactory;

		public ConnectionFactory RabbitMQFactory { get => rabbitMQFactory; }

        //defaults in args
        private MQConnectionFactory(string host="localhost", int port=5672)
		{
            rabbitMQFactory = new ConnectionFactory() { HostName = host, Port = port };
		}

        public static MQConnectionFactory getInstance()
        {
            return instance;
        }
    }
}
