using MassTransit;

namespace Consumer.Consumers
{
	public interface IConsumer<in TMessage> :
		IConsumer
		where TMessage : class
	{
		Task Consume(ConsumeContext<TMessage> context);
	}
}
