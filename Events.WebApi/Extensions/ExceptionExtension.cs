using Events.WebApi.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Events.WebApi.Extensions
{
    internal static class ExceptionExtension
    {
        internal static ExceptionDetails GetExceptionDetails(
            this Exception ex)
        {
            ArgumentNullException.ThrowIfNull(ex, nameof(ex));

            return ex switch
            {
                ValidationException
                    => new ExceptionDetails(
                        HttpStatusCode.BadRequest,
                        "Validation Failed",
                        ex.Message),

                KeyNotFoundException
                    => new ExceptionDetails(
                        HttpStatusCode.NotFound,
                        "Not Found",
                        ex.Message),

                _ => new ExceptionDetails(
                        HttpStatusCode.InternalServerError,
                        "Internal Server Error",
                        ex.Message)
            };
        }
    }
}