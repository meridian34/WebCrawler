using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WebCrawler.WebApi.Exceptions;

namespace WebCrawler.WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        private readonly JsonSerializerOptions _serializerOptions;

        public ExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException e)
            {
                await AddExceptionInfoToRespose(400, e, context);
            }
            catch (Exception e)
            {
                await AddExceptionInfoToRespose(400, e, context);
            }
        }

        private async Task AddExceptionInfoToRespose(int statusCode, Exception exception, HttpContext context)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var responseObject = await GetResponseObject(exception);
            var serialized = JsonSerializer.Serialize(responseObject, _serializerOptions);

            await context.Response.WriteAsync(serialized);
        }

        private async Task<Object> GetResponseObject(Exception exception)
        {
            if (_environment.IsDevelopment())
            {

                return new
                {
                    Message = exception.Message,
                    Type = exception.GetType().FullName,
                    StackTrace = exception.StackTrace
                };
            }
            else
            {
                return new
                {
                    Message = exception.Message
                };
            }
        }
    }
}
