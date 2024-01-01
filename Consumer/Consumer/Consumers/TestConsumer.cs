using Consumer.Model;
using MassTransit;

namespace Consumer.Consumers
{
	public class TestConsumer : IConsumer<TestMessage>
	{
		public Task Consume(ConsumeContext<TestMessage> context)
		{
			Console.WriteLine(context.Message);

			return Task.CompletedTask;
		}
	}
}
