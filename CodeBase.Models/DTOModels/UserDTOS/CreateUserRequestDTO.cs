namespace CodeBase.Models.DTOModels.UserDTOS;
public class CreateUserRequestDTO
{
    [Required]
    public string Password { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
