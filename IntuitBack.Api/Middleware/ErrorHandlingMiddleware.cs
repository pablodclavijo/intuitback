using IntuitBack.Application.Interfaces;
using System.Net;
using System.Text.Json;

namespace IntuitBack.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogService logService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await logService.RegistrarAsync("Error", ex.Message, ex.StackTrace);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = "Internal server error" });
            await context.Response.WriteAsync(result);
        }
    }
}
