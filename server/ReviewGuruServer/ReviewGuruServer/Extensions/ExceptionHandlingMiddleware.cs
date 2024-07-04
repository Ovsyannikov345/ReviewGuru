using ReviewGuru.API.Utilities.Responses;
using ReviewGuru.BLL.Utilities.Exceptions;
using System.Net;

namespace ReviewGuru.API.Extensions
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ExceptionResponse response = ex switch
            {
                BadRequestException => new(HttpStatusCode.BadRequest, ex.Message),
                ForbiddenException => new(HttpStatusCode.Forbidden, ex.Message),
                InternalServerErrorException => new(HttpStatusCode.InternalServerError, ex.Message),
                NotFoundException => new(HttpStatusCode.NotFound, ex.Message),
                UnauthorizedException => new(HttpStatusCode.Unauthorized, ex.Message),
                _ => new(HttpStatusCode.InternalServerError, ex.Message),
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
