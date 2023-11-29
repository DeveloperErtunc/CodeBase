namespace CodeBase.API.Controllers;
public class OrderController : BaseController
{
   readonly IOrderService _orderService;
    public OrderController(IMapper mapper, IOrderService orderService) : base(mapper)
    {
        _orderService = orderService;
    }
    [Route("CreateOrderItem")]
    [HttpPost]
    [Authorize(Roles = "Customer")]
    [EnableRateLimiting("CreateOrderPolicy")]
    public async Task<CommandResult> CreateOrderItem(CreateOrderItemDTO model) => await _orderService.CreateOrderItem(model);
    [Route("Create")]
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<CommandResult<OrderDTO>> Create() => await _orderService.CreateOrder();
    [Route("GetById/{id}")]
    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<CommandResult<OrderDTO>> GetById(Guid id) => await _orderService.GetOrderById(id);

    [Route("GetAllOrders")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<CommandResult<PaginationModel<OrderDTO>>> GetAllOrders(PaginationDTO model) => await _orderService.GetAllOrders(model);
}
