namespace CodeBase.Services.Services;
public class UserService : IUserService
{
    readonly UserManager<ApplicationUser> _userManager;
    readonly IMapper _mapper;
    readonly JwtSettings _jwtSettings;
    public UserService(UserManager<ApplicationUser> appUser, IMapper mapper, JwtSettings jwtSettings, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = appUser;
        _mapper = mapper;
        _jwtSettings = jwtSettings;
    }
    public async Task<CommandResult> CreateUser(CreateUserRequestDTO request)
    {
        try
        {
            var appUser = _mapper.Map<ApplicationUser>(request);
            var result = await _userManager.CreateAsync(appUser, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(appUser, AccountRoles.Customer.ToString());
                return CommandResult.GetSucceed();
            }
            return CommandResult.GetFailed(string.Join(",", result?.Errors?.Select(x => x.Description) ?? new List<string>()));
        }
        catch (Exception ex)
        {
            return CommandResult.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
    public async Task<CommandResult<PaginationModel<CustomerDTO>>> GetCustomers(PaginationDTO model)
    {
        try
        {
            var users = _userManager.Users.Include(x => x.UserRoles).ThenInclude(x=>x.Role).AsNoTracking();
                users = users.Where(x =>x.UserRoles != null && !x.UserRoles.Any(a => a.Role.Name == AccountRoles.Admin.ToString()));
            var customers = await users.Select(x => new CustomerDTO
            {
                Email = x.Email,
                FullName = x.Name + " " + x.Surname,
                UserId = x.Id,
                Roles = x.UserRoles.Select(x => x.Role.Name).ToList()
            }).Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return CommandResult<PaginationModel<CustomerDTO>>.GetSucceed(new PaginationModel<CustomerDTO>(customers, await users.CountAsync(), model.PageIndex, model.PageSize));
        }
        catch (Exception ex)
        {
            return CommandResult<PaginationModel<CustomerDTO>>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
    public async Task<CommandResult<UserResponseDTO>> Login(LoginUserRequestDTO request)
    {
        try
        {
            var user = await _userManager.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
                return CommandResult<UserResponseDTO>.NotFound();

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return CommandResult<UserResponseDTO>.GetFailed(ErrorMessageConstants.PASSWORD_WRONG);

            var userRoles = user.UserRoles?.Select(x => x.Role.Name)?.ToList() ?? new List<string?>();
            var currentUser = new CurrentUser(user.Id, user.Email, user.Name + " " + user.Surname, userRoles);
            var token = JwtHelper.CreateToken(_jwtSettings, TokenModel.GetClaims(currentUser));
            return CommandResult<UserResponseDTO>.GetSucceed(new UserResponseDTO(currentUser.UserId, currentUser.FullName, currentUser.UserMail, userRoles, token));
        }
        catch (Exception ex)
        {
            return CommandResult<UserResponseDTO>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
}
