namespace CodeBase.Models.ViewModels;
public class TokenModel
{
    public string UserId { get; }
    public string FullName { get; }
    public string Email { get; }
    public string AccountType { get; set; }
    public TokenModel(ClaimsPrincipal claimsPrincipal)
    {
        
        UserId = claimsPrincipal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        AccountType = claimsPrincipal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value ?? string.Empty;
        Email = claimsPrincipal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value ?? "";
        FullName = claimsPrincipal.Claims.FirstOrDefault(p => p.Type == "fullname")?.Value ?? "";
    }
    public static IEnumerable<Claim> GetClaims(CurrentUser account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.UserId.ToString()),
            new Claim(ClaimTypes.Email, account.UserMail),
     
            new Claim("fullname", account.FullName),
        };
        if(account.AccountRoles != null)
        {
            foreach (var item in account?.AccountRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
        }
    
        return claims;
    }
}
