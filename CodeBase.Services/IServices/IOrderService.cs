namespace CodeBase.Services.IServices;
public interface IOrderService:IServiceBase<Order>
{
    Task<CommandResult<OrderDTO>> CreateOrderItem(CreateOrderItemDTO model);
    Task<CommandResult<OrderDTO>> CreateOrder();
    Task<CommandResult<PaginationModel<OrderDTO>>> GetAllOrders(PaginationDTO model);
    Task<CommandResult<OrderDTO>> GetOrderById(Guid id);
    Task<CommandResult<List<OrderStatistics>>> GetOrderStatistics(string? userId);
}
