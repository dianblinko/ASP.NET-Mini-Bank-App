using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Minibank.Core;

namespace Minibank.Web.Middlewares
{
    public class UserFriendlyExceptionMiddleware
    {
        public readonly RequestDelegate next;

        public UserFriendlyExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (UserFriendlyException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message});
            }
        }
    }
}
