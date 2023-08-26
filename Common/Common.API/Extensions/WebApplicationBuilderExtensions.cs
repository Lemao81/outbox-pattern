using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Common.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder, string serviceName)
    {
        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ServiceName", serviceName)
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();
        builder.Logging.AddSerilog(logger);

        return builder;
    }
}
