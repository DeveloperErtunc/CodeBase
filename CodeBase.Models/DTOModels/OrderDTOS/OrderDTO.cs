namespace CodeBase.Models.DTOModels.OrderDTOS;
public class OrderDTO
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
    public string CreatedOn { get; set; }
    public string AppUserId { get; set; }
    public string UserName { get; set; }
}
public class OrderItemDTO
{
    public Guid OrderId { get; set; }
    public Guid BookId { get; set; }
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice => Price * Quantity;
}
public class OrderStatistics
{
    public string Month { get; set; }
    public int TotalOrderCount { get; set; }
    public int TotalBookCount { get; set; }
    public decimal Amount { get; set; }
}