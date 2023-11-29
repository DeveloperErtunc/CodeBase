namespace CodeBase.Models.ConfigModels;
public class JwtSettings
{
    public string JwtSecret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int TokenExpireDay { get; set; }
}
public class GetSettings
{
    public JwtSettings JwtSettings { get; set; }
}