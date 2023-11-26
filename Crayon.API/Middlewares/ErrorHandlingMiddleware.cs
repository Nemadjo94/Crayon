using Crayon.Application.Exceptions;

using FluentValidation;

using Newtonsoft.Json;

using System.Net;

namespace Crayon.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string? message = null;

            switch (ex)
            {
                case RestException restException:
                    _logger.LogError(ex, "REST ERROR");
                    message = restException.Message;
                    context.Response.StatusCode = (int)restException.Code;
                    break;

                case ValidationException validationException:
                    _logger.LogError(ex, "VALIDATION ERROR");
                    message = validationException.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case Exception exception:
                    _logger.LogError(ex, "SERVER ERROR");
                    message = string.IsNullOrWhiteSpace(exception.Message) ? "Error" : exception.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            if (!string.IsNullOrWhiteSpace(message))
            {
                var result = JsonConvert.SerializeObject(new
                {
                    message
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}
