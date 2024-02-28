using System.Net;
using System.Text.Json;
using Serilog;
using Serilog.Context;

namespace Sample.IExceptionHandler.Middlewares;

public class ErrorHandlingMiddleware : Microsoft.AspNetCore.Diagnostics.IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		using var property = LogContext.PushProperty("UserName", httpContext.User?.Identity?.Name ?? "anônimo");
		Log.Error(exception, "Error");

		var result = JsonSerializer.Serialize(new { error = exception?.Message });

		httpContext.Response.ContentType = "application/json";
		httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		await httpContext.Response.WriteAsync(result, cancellationToken: cancellationToken);

		return true;
	}
}