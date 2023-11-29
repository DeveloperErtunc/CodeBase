namespace CodeBase.Services.IServices;
public interface IUserService
{
    Task<CommandResult> CreateUser(CreateUserRequestDTO request);
    Task<CommandResult<UserResponseDTO>> Login(LoginUserRequestDTO request);
    Task<CommandResult<PaginationModel<CustomerDTO>>> GetCustomers(PaginationDTO model);
}
