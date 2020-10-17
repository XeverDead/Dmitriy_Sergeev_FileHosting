using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileHosting
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _validToken;

        public AuthenticationMiddleware(RequestDelegate next, string validToken)
        {
            _next = next;
            _validToken = validToken;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Query["Token"].Equals(_validToken))
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 403;
            }
        }
    }
}
