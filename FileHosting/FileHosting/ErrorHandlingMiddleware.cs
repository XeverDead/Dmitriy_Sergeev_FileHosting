using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileHosting
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);

            if (context.Response.StatusCode == 403)
            {
                await context.Response.WriteAsync("Nizzya (a token to secret)");
            }
            else if (context.Response.StatusCode == 404)
            {
                await context.Response.WriteAsync("Netu takoy stranicy");
            }
        }
    }
}
