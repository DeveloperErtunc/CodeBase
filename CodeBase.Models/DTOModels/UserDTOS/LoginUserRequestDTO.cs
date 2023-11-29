namespace CodeBase.Models.DTOModels.UserDTOS;
public class LoginUserRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
