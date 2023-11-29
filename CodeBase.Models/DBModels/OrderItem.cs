namespace CodeBase.Models.DBModels;
public class OrderItem:BaseEntity
{
    public OrderItem(Guid bookId, int quantity,string userId,decimal price)
    {
        BookId = bookId;
        Quantity = quantity;
        Price = price;
        Created(userId);
    }
    public OrderItem()
    {
        
    }
    public Guid OrderId { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public virtual Order? Order { get; set; }
    public virtual Book? Book { get; set; }
}
