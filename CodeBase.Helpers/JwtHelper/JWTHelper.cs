namespace CodeBase.Helpers.JWT;
public static class JwtHelper
{
    internal static string CreateJwtToken(JwtSettings settings, DateTime expires,
                                        IEnumerable<Claim> claims, string secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var token = new JwtSecurityToken(
           issuer: settings.Issuer,
           audience: settings.Audience,
           expires:expires,
           claims: claims,
           signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
           );
        return tokenHandler.WriteToken(token);
    }
    public static string CreateJwtToken(JwtSettings settings,
                                        IEnumerable<Claim> claims)
    {
        return CreateJwtToken(settings, DateTime.UtcNow.AddDays(settings.TokenExpireDay), claims, settings.JwtSecret);
    }

    public static ClaimsPrincipal GetPrincipalFromToken(JwtSettings settings, string token, string secret)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = settings.Audience,
            ValidIssuer = settings.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                StringComparison.InvariantCulture) || jwtSecurityToken.ValidTo <= DateTime.UtcNow)
                throw new SecurityTokenException(ErrorMessageConstants.TOKEN_INVALID);

            return principal;

        }
        catch (Exception ex)
        {

            throw new SecurityTokenException(ErrorMessageConstants.TOKEN_INVALID);

        }
    }
}