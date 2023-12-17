using Core.Services;

namespace TwitterCloneAPI.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Authorize(context);
        await _next(context);
    }

    private static void Authorize(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault();

        token = token
            ?.Split(new string[] { " " }, StringSplitOptions.None)
            ?.LastOrDefault();

        var tokenService = context.RequestServices.GetRequiredService<ITokenService>();
        var userGuid = tokenService.GetUserGuid(token);
        context.Items["Guid"] = userGuid.Data;
    }
}
