namespace CodeBase.Services.Middleware;
public class ApiAuthMiddleware
{
    private readonly RequestDelegate _next;
    public ApiAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context, ICurrentUserService userService)
    {
        var token = context?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
            userService.Authenticate(token);
        else
            userService.Logout();

        await _next(context);
    }
}
