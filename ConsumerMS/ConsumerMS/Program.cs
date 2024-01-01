using System;
using System.Threading.Tasks;
using MassTransit;
using RabbitMQ.Client;
using ConsumerMS;
using System.Threading;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
	cfg.ReceiveEndpoint("test-event", e =>
	{
		e.Consumer<TestConsumer>();
		e.ExchangeType = ExchangeType.Direct;
		e.UseMessageRetry(r =>
		{
			r.Interval(2, TimeSpan.FromMinutes(1));
		});
	});
});

await busControl.StartAsync(new CancellationToken());

try
{
	Console.WriteLine("Pressione enter para sair");

	await Task.Run(() => Console.ReadLine());
}
finally 
{
	await busControl.StopAsync();
}
