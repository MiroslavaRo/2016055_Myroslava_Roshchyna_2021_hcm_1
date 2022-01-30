using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Hcm.Web.Middlewares
{
    public class AuthenticationCookieMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Cookies.TryGetValue("AuthCookie", out var value))
            {
                context.Request.Headers.Add("Authorization", $"Bearer {value}");
            }

            return next(context);
        }
    }
}
