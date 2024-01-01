using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Producer.DTO;
using Producer.Model;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string QUEUE_NAME = "test-message"; 

app.MapPost("/test-message", (TestDTO testDTO) =>
{
	try
	{
		var connectionFactory = new ConnectionFactory
		{
			HostName = "localhost",
		};

		using var connection = connectionFactory.CreateConnection();

		using var channel = connection.CreateModel();

		//channel.ConfirmSelect();

		//channel.BasicAcks += Channel_BasicAcks;
		//channel.BasicNacks += Channel_BasicNacks;

		channel.QueueDeclare(
				queue: QUEUE_NAME,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);

		var testMessage = new Test(testDTO.Message, testDTO.Sucesso);
		var serialize = JsonConvert.SerializeObject(testMessage, new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver()
		});
		var body = Encoding.UTF8.GetBytes(serialize);

		channel.BasicPublish(
			exchange: "",
			routingKey: QUEUE_NAME,
			basicProperties: null,
			body: body);

		return Results.Ok();
	}
	catch (Exception)
	{
		return Results.BadRequest();
	}
})
.WithOpenApi();

//void channel_basicacks(object? sender, basicackeventargs e)
//{
//	console.writeline($"{datetime.utcnow} -> falha");
//}

//void channel_basicnacks(object? sender, basicnackeventargs e)
//{
//	console.writeline($"{datetime.utcnow} -> sucesso");
//}

app.Run();
