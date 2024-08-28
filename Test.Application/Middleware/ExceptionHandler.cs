using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Test.Application.Dto;

namespace Test.Application.ExceptionHandler
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.NotFound,
                    "Не найдено");
            }
            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    "Аргумент не может быть пустым");
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    "Недопустимая операция");
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.Forbidden,
                    "Доступ запрещен");
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    "Неверный аргумент");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.InternalServerError,
                    "Внутренняя ошибка сервера");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string exMsg, HttpStatusCode httpStatusCode, string message)
        {
            _logger.LogError(exMsg);

            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            ErrorDto errorDto = new()
            {
                Message = message,
                StatusCode = (int)httpStatusCode
            };

            await response.WriteAsJsonAsync(errorDto);
        }
    }
}
