using System.Text.Json;
using Muzzuf.Service.CustomError;

namespace Muzzuf.Middleware
{
    public class GlobalExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
               await _next(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);

                int statusCode = StatusCodes.Status500InternalServerError;
                string message = "Internal Server Something went wrng";

                if(ex is AppException appEx)
                {
                    message = appEx.Message;
                    statusCode = appEx.StatusCode;

                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message,
                    statusCode,
                };
               
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

    }
}
