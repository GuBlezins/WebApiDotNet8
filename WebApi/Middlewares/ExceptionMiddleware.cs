using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu uma exceção não tratada.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode status;
        string message;

        if (exception is UnauthorizedAccessException)
        {
            status = HttpStatusCode.Unauthorized;
            message = exception.Message;
        }
        else if (exception is ArgumentException || exception is InvalidOperationException || exception is KeyNotFoundException)
        {
            status = HttpStatusCode.BadRequest;
            message = exception.Message;
        }
        else
        {
            status = HttpStatusCode.InternalServerError;
            message = "Ocorreu um erro interno no servidor.";
        }

        context.Response.StatusCode = (int)status;

        var response = new
        {
            error = message,
            status = context.Response.StatusCode
        };

        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}
