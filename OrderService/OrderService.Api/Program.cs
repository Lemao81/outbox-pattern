using Common.API.Extensions;
using Common.Domain.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.OrderService);

var app = builder.Build();

app.Run();
