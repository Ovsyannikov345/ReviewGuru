using System.Net;

namespace ReviewGuru.API.Utilities.Responses
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message)
}
