using System.Net.Sockets;
using Common.API.Extensions;
using Common.Domain.Consts;
using Common.Domain.Models;
using DeliveryService.API.HostedServices;
using DeliveryService.Domain.Db;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.DeliveryService);

builder.Services.AddDbContextFactory<DeliveryServiceDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddHostedService<MessageConsumingHostedService>();

builder.Services.AddRabbitMqConnection();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
    Policy.Handle<SocketException>().Or<PostgresException>(e => e.Message.Contains("system is starting up")).WaitAndRetry(10, _ => TimeSpan.FromSeconds(2))
        .Execute(() =>
        {
            Console.WriteLine("Try migrate database");
            dbContext.Database.Migrate();
        });
}

app.Run();
