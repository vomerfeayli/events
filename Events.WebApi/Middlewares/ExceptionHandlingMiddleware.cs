using Events.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Events.WebApi.Middlewares
{
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

        public async Task Invoke(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while execute: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception ex)
        {
            ArgumentNullException.ThrowIfNull(ex);

            var exceptionDetails = ex.GetExceptionDetails();

            context.Response.StatusCode = (int)exceptionDetails.StatusCode;

            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = (int)exceptionDetails.StatusCode,
                    Title = exceptionDetails.Title,
                    Detail = exceptionDetails.Message
                });
        }
    }
}