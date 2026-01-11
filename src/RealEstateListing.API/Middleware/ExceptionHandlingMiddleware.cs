using System.Net;
using System.Text.Json;
using RealEstateListing.Application;
using RealEstateListing.Domain;

namespace RealEstateListing.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation error");
            await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Application.ApplicationException ex)
        {
            _logger.LogWarning(ex, "Application error: {ErrorCode}", ex.ErrorCode);
            var statusCode = MapErrorCodeToStatusCode(ex.ErrorCode);
            await HandleExceptionAsync(context, ex.Message, statusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, "An unexpected error occurred", HttpStatusCode.InternalServerError);
        }
    }

    private static HttpStatusCode MapErrorCodeToStatusCode(ServiceErrorCode errorCode) => errorCode switch
    {
        ServiceErrorCode.NotFound => HttpStatusCode.NotFound,
        ServiceErrorCode.Conflict => HttpStatusCode.Conflict,
        ServiceErrorCode.ValidationFailed => HttpStatusCode.UnprocessableEntity,
        _ => HttpStatusCode.BadRequest
    };

    private static async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new { error = message };
        var json = JsonSerializer.Serialize(response);
        
        await context.Response.WriteAsync(json);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
