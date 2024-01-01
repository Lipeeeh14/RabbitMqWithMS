using MassTransit;
using SubmitTest;
using System;
using System.Threading.Tasks;

namespace ConsumerMS
{
	public class TestConsumer : IConsumer<ISubmitTest>
	{
		public TestConsumer()
		{
		}

		public async Task Consume(ConsumeContext<ISubmitTest> context)
		{
			Console.WriteLine($"To tentando enviar");
			throw new Exception("Simulando uma exceção durante o processamento da mensagem");
			//Console.WriteLine($"Chegou -> {JsonConvert.SerializeObject(context.Message)}");
		}
	}
}
