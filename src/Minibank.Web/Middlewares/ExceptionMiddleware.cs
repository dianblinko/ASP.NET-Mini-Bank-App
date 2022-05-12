using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Core;

namespace Minibank.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (ObjectNotFoundException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
            catch (FluentValidation.ValidationException exception)
            {
                var errors = exception.Errors.Select(x => $"{x.ErrorMessage}");
                var errorMessage = string.Join(Environment.NewLine, errors);
                await httpContext.Response.WriteAsJsonAsync(new { Message = errorMessage });
            }
            catch (ValidationException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
            catch (Exception exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new { Message = "Внутренняя ошибка сервера" });
            }
        }
    }
}
