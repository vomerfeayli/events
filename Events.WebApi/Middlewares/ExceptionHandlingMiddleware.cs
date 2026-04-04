using Events.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Events.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(
            RequestDelegate next)
        {
            _next = next;
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception ex)
        {
            ArgumentNullException.ThrowIfNull(ex);

            var exceptionDetails = ex.GetExceptionDetails();

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