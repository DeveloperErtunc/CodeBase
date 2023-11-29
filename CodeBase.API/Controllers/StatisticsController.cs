namespace CodeBase.API.Controllers;
public class StatisticsController : BaseController
{
    readonly IOrderService _orderService;
    readonly ICurrentUserService _currentUser;

    public StatisticsController(IMapper mapper, IOrderService orderService, ICurrentUserService currentUser) : base(mapper)
    {
        _orderService = orderService;
        _currentUser = currentUser;
    }

    [Route("GetStatistics")]
    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<CommandResult<List<OrderStatistics>>> GetStatistics() => await _orderService.GetOrderStatistics(_currentUser.GetUserId());

    [Route("GetAllStatistics")]
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<CommandResult<List<OrderStatistics>>> GetAllStatistics() => await _orderService.GetOrderStatistics(null);
}
