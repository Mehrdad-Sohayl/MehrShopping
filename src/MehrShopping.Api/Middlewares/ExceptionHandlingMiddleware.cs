
using MehrShopping.Application.Exceptions;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Infrastructure.Exceptions;
using ApplicationException = MehrShopping.Application.Exceptions.ApplicationException;

namespace MehrShopping.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = ex switch
            {
                ApplicationException => StatusCodes.Status400BadRequest,
                InfrastructureException => StatusCodes.Status500InternalServerError,
                DomainException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                error = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
