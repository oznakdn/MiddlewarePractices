using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MiddlewarePractices.Middlewares
{
    public class HelloMiddleware
    {

        private readonly RequestDelegate _next;
        public HelloMiddleware(RequestDelegate next)
        {
            _next=next;
        }

        public async Task Invoike(HttpContext context)
        {
            Console.WriteLine("Hello world.");
            await _next.Invoke(context);
            Console.WriteLine("Bye World.");
        }

        
    }

    static public class HelloMiddlewareExtension
        {
            public static IApplicationBuilder UseHello(this IApplicationBuilder builder)
            {
                 return builder.UseMiddleware<HelloMiddleware>();
            }
        }
}