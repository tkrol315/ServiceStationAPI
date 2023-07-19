using System.Diagnostics;

namespace ServiceStationAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch sw = Stopwatch.StartNew();
            await next.Invoke(context);
            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            if (elapsed > 4000)
            {
                _logger.LogWarning($"Request {context.Request.Method} at path {context.Request.Path} took {elapsed} ms");
            }
        }
    }
}