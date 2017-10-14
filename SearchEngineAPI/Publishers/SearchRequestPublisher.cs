using System;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SearchEngineAPI.Factories;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngineAPI.Publishers
{
    public class SearchRequestPublisher : IPublishers
    {
        private BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private ConcurrentQueue<string> respConQueue = new ConcurrentQueue<string>();
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private string corrId;
        private EventingBasicConsumer consumer;
        private IBasicProperties props;

        public SearchRequestPublisher(Boolean bAsync = false)
        {
			//initialize connections and channels
			factory = MQConnectionFactory.getInstance().RabbitMQFactory;
			connection = factory.CreateConnection();
			channel = connection.CreateModel();

			//set up consumer event for when message is received
			consumer = new EventingBasicConsumer(channel);

			if (bAsync)
			{
				//declare rpc queue if not done already
				channel.QueueDeclare("rpc_async_queue", false, false, false, null);
				consumer.Received += (model, ea) => ConsumerEventAsync(model, ea);
			}
			else
			{
				//declare rpc queue if not done already
				channel.QueueDeclare("rpc_queue", false, false, false, null);
				consumer.Received += (model, ea) => ConsumerEvent(model, ea);
			}

			//declare reply queue
			replyQueueName = channel.QueueDeclare().QueueName;

			//set up message properties
			corrId = Guid.NewGuid().ToString();
			props = channel.CreateBasicProperties();
			props.ReplyTo = replyQueueName;
			props.CorrelationId = corrId;

			
        }

        public string GetMessage(string query)
        {
            //serialize request
            var message = Encoding.UTF8.GetBytes(query);

            //publish request to the rpc queue
            channel.BasicPublish("", "rpc_queue", props, message);

            //consume reply from reply queue and execute consumer event
            channel.BasicConsume(consumer, replyQueueName, true);

            //return response from queue
            return respQueue.Take();
        }

        public async Task<string> GetMessageAsync(string query)
        {
            //serialize request
            var message = Encoding.UTF8.GetBytes(query);

            //publish request to the rpc queue
            channel.BasicPublish("", "rpc_async_queue", props, message);

            //consume reply from reply queue and execute consumer event
            channel.BasicConsume(consumer, replyQueueName, true);

            //return response from queue
            //string res;
            //respConQueue.TryDequeue(out res);
            return respQueue.Take();
            //return res;
        }

        private void ConsumerEvent(object model, BasicDeliverEventArgs ea)
        {
			//deserialize response
		    var body = ea.Body;
		    var response = Encoding.UTF8.GetString(body);

            //if correlationid is same, message is response to a reply
            if (ea.BasicProperties.CorrelationId == corrId)
            {
                //blocking response queue to handle responses synchronously
                respQueue.Add(response);
            }
        }

        private void ConsumerEventAsync(object model, BasicDeliverEventArgs ea)
		{
			//deserialize response
			var body = ea.Body;
			var response = Encoding.UTF8.GetString(body);

			//if correlationid is same, message is response to a reply
			if (ea.BasicProperties.CorrelationId == corrId)
			{
				//blocking response queue to handle responses synchronously
				//respConQueue.Enqueue(response);
                respQueue.Add(response);
			}
		}

		public void CloseConnection()
		{
            //clean up connections
            channel.Close();
			connection.Close();
		}
    }
}
