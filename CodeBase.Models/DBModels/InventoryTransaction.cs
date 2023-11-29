namespace CodeBase.Models.DBModels;
public class InventoryTransaction : BaseEntity
{
    public InventoryTransaction(int transactionStock, string userId ,Guid? orderId = null)
    {
        TransactionStock = transactionStock;
        OrderId = orderId;
        Created(userId);
    }
    public InventoryTransaction()
    {
        
    }
    public int TransactionStock { get; set; }
    public Guid BookId { get; set; }
    public Guid? OrderId { get; set; }

    public virtual Book? Book { get; set; }
    public virtual Order? Order { get; set; }
}
