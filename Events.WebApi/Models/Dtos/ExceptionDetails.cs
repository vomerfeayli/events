using System.Net;

namespace Events.WebApi.Models.Dtos
{
    internal record ExceptionDetails(
        HttpStatusCode StatusCode,
        string Title,
        string Message);
}