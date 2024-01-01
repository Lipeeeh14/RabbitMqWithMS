using Consumer.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer.Service
{
	public class MessageService : IMessageService
	{
		private readonly ConnectionFactory _factory;
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private readonly string QUEUE_NAME = "test-message";

		public MessageService()
		{
			_factory = new ConnectionFactory
			{
				HostName = "localhost",
			};

			_connection = _factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(
				queue: QUEUE_NAME,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null);
		}

		public Task ConsumeMessage()
		{
			var consumer = new EventingBasicConsumer(_channel);

			consumer.Received += (sender, args) =>
			{
				var contentArray = args.Body.ToArray();
				var contentString = Encoding.UTF8.GetString(contentArray);
				//TestMessage? test = JsonConvert.DeserializeObject<TestMessage>(contentString);

				// deu erro
				//_channel.BasicNack(args.DeliveryTag, false, true);

				// deu certo
				_channel.BasicAck(args.DeliveryTag, false);

				Console.WriteLine(contentString);
			};

			_channel.BasicConsume("test-message", false, consumer);

			return Task.CompletedTask;
		}
	}
}
