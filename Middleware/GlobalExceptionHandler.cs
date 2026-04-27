using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                KeyNotFoundException     => (StatusCodes.Status404NotFound,            "Not Found"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,     "Unauthorized"),
                InvalidOperationException => (StatusCodes.Status400BadRequest,          "Bad Request"),
                ArgumentException        => (StatusCodes.Status400BadRequest,           "Bad Request"),
                _                        => (StatusCodes.Status500InternalServerError,  "Internal Server Error")
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
                _logger.LogError(exception, "Unhandled exception on {Method} {Path}", context.Request.Method, context.Request.Path);
            else
                _logger.LogWarning(exception, "{Title} on {Method} {Path}: {Message}", title, context.Request.Method, context.Request.Path, exception.Message);

            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title  = title,
                Detail = exception.Message
            };

            await context.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }
    }
}
