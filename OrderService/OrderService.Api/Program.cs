using Common.API.Extensions;
using Common.Domain.Consts;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Db;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.OrderService);

builder.Services.AddDbContext<OrderServiceDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderServiceDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
