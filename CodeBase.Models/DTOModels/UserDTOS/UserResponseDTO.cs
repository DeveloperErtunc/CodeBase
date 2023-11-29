namespace CodeBase.Models.DTOModels.UserDTOS;
public class UserResponseDTO
{
    public UserResponseDTO(string userId, string fullName, string email, List<string> accountRoles, string token)
    {
        UserId = userId;
        FullName = fullName;
        Email = email;
        AccountRoles = accountRoles;
        BearerToken = token;
    }
    public string UserId { get; }
    public string FullName { get; }
    public string Email { get; }
    public List<string> AccountRoles { get; set; }
    public string BearerToken { get; set; }
}