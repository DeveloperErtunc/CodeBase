namespace CodeBase.Models.DTOModels.OrderDTOS;
public class CreateOrderDTO
{
    [MinLength(1)]
    public List<CreateOrderItemDTO> OrderItems { get; set; }
}
public class CreateOrderItemDTO
{
    [Required]
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    [Required]
    public Guid OrderId { get; set; }
}