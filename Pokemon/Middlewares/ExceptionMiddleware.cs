using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pokemon.Services.Exceptions;
using Pokemon.Services.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Pokemon.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            string message;
            if (exception is PokemonException)
            {
                var pokemonException = exception as PokemonException;
                context.Response.StatusCode = (int)pokemonException.StatusCode;
                message = pokemonException.Message;
            }
            else
            {
                // not expected exception so we return 500
                message = "Internal Server Error.";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
