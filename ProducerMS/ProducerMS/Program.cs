using MassTransit;
using ProducerMS.DTO;
using SubmitTest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// cfg.Host("amqp://guest:guest@localhost:5672");

builder.Services.AddMassTransit(x => 
{
	x.UsingRabbitMq();
	//x.UsingAzureServiceBus();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/test-message", async (TestDTO testDto, IPublishEndpoint publishEndpoint) =>
{
	await publishEndpoint.Publish<ISubmitTest>(new { testDto.Message });

	Console.WriteLine($"Mensagem enviada --> {testDto.Message}");

	return Results.Ok();
})
.WithOpenApi();

app.Run();
