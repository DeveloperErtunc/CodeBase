namespace CodeBase.Models.DBModels;
public class Order : BaseEntity
{
    public Order()
    {
        
    }
    public Order(string userId)
    {
        AppUserId = userId;
    }
    public string  AppUserId { get; set; }
    public virtual List<OrderItem>? OrderItems { get; set; }
    public virtual ApplicationUser? AppUser { get; set; }
    public OrderItem AddOrderItem(Guid bookId, int quantity,string userId,decimal price)
    {
        OrderItems ??= new List<OrderItem>();
        var orderItem = new OrderItem(bookId, quantity, userId, price);
        OrderItems.Add(orderItem);
        return orderItem;
    }
    public string GetUserName()
    {
        if (AppUser != null)
        {
            return AppUser?.Name + " " + AppUser?.Surname;
        }
        return string.Empty;
    }
}
