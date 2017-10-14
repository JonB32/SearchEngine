using System;
using SearchEngineService.Factories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Collections.Concurrent;
using SearchEngineService.Services;
using System.Collections.Generic;
using SearchEngineService.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SearchEngineService.Publishers
{
    public class SearchRequestPublisher : IPublishers
    {
        ConnectionFactory factory;
		private IConnection connection;
		private IModel channel;

        public SearchRequestPublisher()
        {
			//initialize connections
			factory = MQConnectionFactory.getInstance().RabbitMQFactory;
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

        }

        public void SendMessage()
        {
			//declare rpc queue if not done already
			channel.QueueDeclare("rpc_queue", false, false, false, null);

			//set up quality of service, how messages are transported?
			channel.BasicQos(0, 1, false);

			//set up consumer event for when message is received
			EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

			consumer.Received += (model, ea) =>
			{
				string jsonString = null;

				//grab message properties
				var body = ea.Body;
				var props = ea.BasicProperties;

				//set up reply message properties
				var replyProps = channel.CreateBasicProperties();
				replyProps.CorrelationId = props.CorrelationId;

				try
				{
					//deserialize message
					var message = Encoding.UTF8.GetString(body);

					//body should be query for search, pass query to ES service
                    IReadOnlyCollection<Nerd> response = new DataSourceFactory().GetSearchResults(message);

					//serialize response for reply queue
					jsonString = JsonConvert.SerializeObject(response);
				}
				catch (Exception e)
				{
					//Console.WriteLine(" [.] " + e.Message);
					jsonString = "{\"error\":\"" + e.Message + "\"}";
				}
				finally
				{
					//serialize json to bytes for message
					var responseBytes = Encoding.UTF8.GetBytes(jsonString);

					//publish and acknowledge response to reply queue
					channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
					channel.BasicAck(ea.DeliveryTag, false);
				}
			};

			//consume messages from rpc queue and trigger consumer event
			//channel.BasicConsume("rpc_queue", false, consumer);
			channel.BasicConsume(consumer: consumer, queue: "rpc_queue", autoAck: false);
			//Console.WriteLine(" [x] Awaiting RPC requests");

			//Console.WriteLine(" Press [enter] to exit.");
			//Console.ReadLine();
		}

        public void SendMessageAsync()
		{
			//declare rpc queue if not done already
			channel.QueueDeclare("rpc_async_queue", false, false, false, null);

			//set up quality of service, how messages are transported?
			channel.BasicQos(0, 1, false);

			//set up consumer event for when message is received
			EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
			{
				string jsonString = null;

				//grab message properties
				var body = ea.Body;
				var props = ea.BasicProperties;

				//set up reply message properties
				var replyProps = channel.CreateBasicProperties();
				replyProps.CorrelationId = props.CorrelationId;

				try
				{
					//deserialize message
					var message = Encoding.UTF8.GetString(body);

                    //body should be query for search, pass query to ES service
                    IDataSourceService dataSourceSerive = new DataSourceFactory();
                    IReadOnlyCollection<Nerd> response = await dataSourceSerive.GetSearchResultsAsync(message);

					//serialize response for reply queue
					jsonString = JsonConvert.SerializeObject(response);
				}
				catch (Exception e)
				{
					//Console.WriteLine(" [.] " + e.Message);
					jsonString = "{\"error\":\"" + e.Message + "\"}";
				}
				finally
				{
					//serialize json to bytes for message
					var responseBytes = Encoding.UTF8.GetBytes(jsonString);

					//publish and acknowledge response to reply queue
					channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
					channel.BasicAck(ea.DeliveryTag, false);
				}
			};

			//consume messages from rpc queue and trigger consumer event
			//channel.BasicConsume("rpc_queue", false, consumer);
			channel.BasicConsume(consumer: consumer, queue: "rpc_async_queue", autoAck: false);
			//Console.WriteLine(" [x] Awaiting RPC requests");

			//Console.WriteLine(" Press [enter] to exit.");
			//Console.ReadLine();
		}

		public void CloseConnection()
		{
			//clean up connections
			channel.Close();
			connection.Close();
		}
    }
}
