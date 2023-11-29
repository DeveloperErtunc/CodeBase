namespace CodeBase.Services.Services;
public static class JwtHelper
{
    public static string CreateToken(JwtSettings settings,IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
           issuer: settings.Issuer,
           audience: settings.Audience,
           claims: claims,
           expires: DateTime.UtcNow.AddDays(settings.TokenExpireDay),
           signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static ClaimsPrincipal GetPrincipalFromToken(JwtSettings settings, string token, string secret)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = settings.Audience,
            ValidateIssuer = true,
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
        catch (Exception)
        {

            throw new SecurityTokenException(ErrorMessageConstants.TOKEN_INVALID);

        }
    }

    public static bool IsExpired(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken)
            return true;

        var validTo = jwtToken.ValidTo;
        if (validTo == DateTime.MinValue) return false;
        return validTo <= DateTime.UtcNow;
    }
}