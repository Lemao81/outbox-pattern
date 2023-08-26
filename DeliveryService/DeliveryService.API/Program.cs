using Common.API.Extensions;
using Common.Domain.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.DeliveryService);

var app = builder.Build();

app.Run();
