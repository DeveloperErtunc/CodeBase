namespace CodeBase.Services.Services;
public class CurrentUserService: ICurrentUserService
{
    TokenModel? TokenModel;
    readonly JwtSettings _jwtSettings;
    public CurrentUserService(JwtSettings settings)
    {
        _jwtSettings = settings;
    }
    public void Authenticate(string token)
    {
        try
        {
            var model = JwtHelper.GetPrincipalFromToken(_jwtSettings ,token, _jwtSettings.JwtSecret);
            TokenModel = new TokenModel(model);
        }
        catch (Exception ex)
        {
        }
    }
    public string GetUserId() => TokenModel?.UserId ??string.Empty;
    public bool IsAuthenticated() => TokenModel != null;
    public bool Logout()
    {
        TokenModel = null;
        return true;
    }
}
