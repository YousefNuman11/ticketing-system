using TicketingSystem.API.Common;
using TicketingSystem.Services.Exceptions;

namespace TicketingSystem.API.Middleware
{
    /// <summary>
    /// Converts thrown exceptions into the standard <see cref="ApiResponse"/>
    /// envelope with the correct HTTP status code.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                await WriteAsync(context, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred");
            }
        }

        private static Task WriteAsync(HttpContext context, int statusCode, string message)
        {
            if (context.Response.HasStarted)
                return Task.CompletedTask;

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(ApiResponse.Fail(statusCode, message));
        }
    }
}
