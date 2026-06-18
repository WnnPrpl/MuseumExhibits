using System.Net;
using System.Text.Json;

namespace MuseumExhibits.API
{
    public class ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentException   => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _                   => (int)HttpStatusCode.InternalServerError
            };

            // In development return the real message; in production return a generic one
            // so internal details (file paths, table names, etc.) never reach the client.
            var message = _env.IsDevelopment()
                ? exception.Message
                : context.Response.StatusCode switch
                {
                    (int)HttpStatusCode.BadRequest   => exception.Message,
                    (int)HttpStatusCode.NotFound     => exception.Message,
                    _                                => "An unexpected error occurred."
                };

            var result = JsonSerializer.Serialize(new { message });
            return context.Response.WriteAsync(result);
        }
    }
}
