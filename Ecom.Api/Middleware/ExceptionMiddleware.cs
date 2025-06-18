using Ecom.Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache memoryCache;
        private readonly TimeSpan _ratelimitWindow = TimeSpan.FromSeconds(30);
        private readonly int _ratelimitMaxRequests = 10;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            
              this._next = next;
            this._environment = environment;
            this.memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);
                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = new
                        ApiException((int)HttpStatusCode.TooManyRequests, "Too many request , please try again later");

                    await context.Response.WriteAsJsonAsync(response);
                }
                await _next(context);
            }
            catch (Exception ex)
            {

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _environment.IsDevelopment() ? 
                    new ApiException(statusCode: (int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ApiException((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }


        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var cacheKey = $"Rate:{ip}";
            var now = DateTime.UtcNow;

            var entry = memoryCache.Get<(DateTime timestamp, int count)>(cacheKey);

            if (entry.timestamp != default && now - entry.timestamp < _ratelimitWindow)
            {
                if (entry.count >= _ratelimitMaxRequests)

                {
                    return false;
                }

                memoryCache.Set(cacheKey, (entry.timestamp, entry.count + 1), _ratelimitWindow);
            }
            else
            {
                memoryCache.Set(cacheKey, (now, 1), _ratelimitWindow);
            }

            return true;
        }


        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }

    }
}
