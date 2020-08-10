using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DFlow.EntryPoint.AspNetCore
{
    public class MonitorMiddleware
    {
        private readonly RequestDelegate _next;

        public MonitorMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context, IServiceProvider provider)
        {
            // await QueueMonitor.Instance.Check(provider);
            await _next(context);
        }
    }
}