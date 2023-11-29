namespace CodeBase.Models.DTOModels.BookDTOS;
public class InventoryUpdateDTO
{
    [Range(1,int.MaxValue)]
    public int Quantity { get; set; }
    public Guid BookId { get; set; }
}
