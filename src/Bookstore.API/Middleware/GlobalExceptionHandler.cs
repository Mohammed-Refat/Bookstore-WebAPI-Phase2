using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Middleware;

// IExceptionHandler is the ASP.NET Core 8 built-in interface for global exception handling
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct)
    {
        // Log the exception — always, regardless of type
        _logger.LogError(exception, "Unhandled exception occurred.");

        // Map exception type → correct HTTP status code
        var (statusCode, title) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error")
        };

        // Build a Problem Details response — RFC 7807 standard shape
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;

        // Write the Problem Details as JSON to the response
        await httpContext.Response.WriteAsJsonAsync(problemDetails, ct);

        // Return true = we handled it, don't let ASP.NET Core do anything else
        return true;
    }
}