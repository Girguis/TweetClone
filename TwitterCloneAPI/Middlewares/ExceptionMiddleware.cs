using Application.Response;
using System.Text.Json;

namespace TwitterCloneAPI.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorResponse = new ResponseErrorModel()
            {
                ErrorCode = "internal-error",
                ErrorMessage = "An error occurred while processing your request.",
                StatusCode = 500,
            };

            var json = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(json);
        }
    }
}
