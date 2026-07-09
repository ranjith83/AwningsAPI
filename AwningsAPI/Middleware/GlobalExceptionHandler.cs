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
            // The client (or an intervening proxy) disconnected before we got a chance
            // to respond. Writing a body to an aborted connection just throws a second,
            // unhandled TaskCanceledException, so there's nothing useful left to do.
            if (context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogWarning(exception, "Request aborted by client on {Method} {Path}", context.Request.Method, context.Request.Path);
                return true;
            }

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

            try
            {
                await context.Response.WriteAsJsonAsync(problem, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Connection was aborted mid-write; nothing left to do.
            }

            return true;
        }
    }
}
