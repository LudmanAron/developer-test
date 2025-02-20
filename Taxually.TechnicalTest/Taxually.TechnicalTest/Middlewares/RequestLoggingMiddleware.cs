﻿namespace Taxually.TechnicalTest.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Request started: {method} {url}", context.Request.Method, context.Request.Path);

            await _next(context);

            _logger.LogInformation("Request finished: {method} {url}", context.Request.Method, context.Request.Path);
        }
    }
}
