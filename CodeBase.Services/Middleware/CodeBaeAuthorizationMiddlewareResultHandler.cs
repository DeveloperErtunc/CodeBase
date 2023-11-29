namespace CodeBase.Services.Middleware;
public class CodeBaeAuthorizationMiddlewareResultHandler: IAuthorizationMiddlewareResultHandler
{
    private readonly CodeBaeAuthorizationMiddlewareResultHandler defaultHandler = new();
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Succeeded)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync<CommandResult<string>>(CommandResult<string>.GetFailed(string.Empty));
            return;
        }
    }
}
