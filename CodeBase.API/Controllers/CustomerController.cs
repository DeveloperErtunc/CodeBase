namespace CodeBase.API.Controllers;
public class CustomerController : BaseController
{
    readonly  IUserService _userService;
    readonly IOrderService _orderService;
    readonly ICurrentUserService _currentUserService;
    public CustomerController(IMapper mapper, IUserService userService, IOrderService orderService, ICurrentUserService currentUserService) : base(mapper)
    {
        _userService = userService;
        _orderService = orderService;
        _currentUserService = currentUserService;
    }
    [Route("Create")]
    [HttpPost]
    public async Task<CommandResult> CreateUser(CreateUserRequestDTO request)=> await _userService.CreateUser(request);
    [Route("Login")]
    [HttpPost]
    public async Task<CommandResult> Login(LoginUserRequestDTO request) => await _userService.Login(request);

    [Route("Customers")]
    [Authorize(Roles ="Admin")]
    [HttpPost]
    public async Task<CommandResult> GetCustomers(PaginationDTO request) => await _userService.GetCustomers(request);

    [Route("GetMyOrders")]
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<CommandResult<PaginationModel<OrderDTO>>> GetMyOrders(PaginationDTO model) => await _orderService.GetAllOrders(new PaginationDTO { UserId = _currentUserService.GetUserId(), PageSize = model.PageSize, PageIndex = model.PageIndex });
}
