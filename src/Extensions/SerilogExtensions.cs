using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;

namespace Sample.IExceptionHandler.Extensions;

public static class SerilogExtension
{
	public static void AddSerilogApi(this WebApplicationBuilder builder, IConfiguration configuration)
	{
		var logLevel = builder.Environment.IsProduction() ? LogEventLevel.Warning : LogEventLevel.Debug;

		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
			.Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
			.Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
			.Enrich.FromLogContext()
			.Enrich.WithExceptionDetails()
			.Enrich.WithCorrelationId()
			.Enrich.WithCorrelationIdHeader()
			.Enrich.WithProperty("ApplicationName", "API Serilog")
			.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}", restrictedToMinimumLevel: LogEventLevel.Debug)
			.CreateLogger();

		builder.Logging.ClearProviders();
		builder.Host.UseSerilog(Log.Logger, true);
	}
}