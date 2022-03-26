using Microsoft.AspNetCore.Http;
using Minibank.Core;
using System.Threading.Tasks;

namespace Minibank.Web.Middlewares
{
    public class ObjectNotFoundExceptionMiddleware
    {
        public readonly RequestDelegate next;

        public ObjectNotFoundExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (ValidationException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
        }
    }
}
