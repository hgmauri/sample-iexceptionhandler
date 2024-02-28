using Sample.IExceptionHandler.Extensions;
using Sample.IExceptionHandler.Middlewares;
using Serilog;

try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.AddSerilogApi(builder.Configuration);

	builder.Services.AddControllers();
	builder.Services.AddSwaggerGen();
	builder.Services.AddEndpointsApiExplorer();

	builder.Services.AddExceptionHandler<ErrorHandlingMiddleware>();
	builder.Services.AddProblemDetails();

	var app = builder.Build();
	app.UseSwagger();
	app.UseSwaggerUI();

	app.UseExceptionHandler(opt => { });

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
	Log.Information("Server Shutting down...");
	Log.CloseAndFlush();
}
