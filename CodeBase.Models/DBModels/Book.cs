namespace CodeBase.Models.DBModels;
public class Book: BaseEntity
{
    public Book(string name, decimal price, string author, int stock,string userId)
    {
        Name = name;
        Price = price;
        Author = author;
        Stock = stock;
        InventoryTransactions = new List<InventoryTransaction>() { new(stock,userId, null) };
    }
    public Book()
    {
        
    }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Author { get; set; }
    public int Stock { get; set; }
    public virtual  List<InventoryTransaction>? InventoryTransactions { get; set; }
    public void UpdateStock(int stock,Guid? orderId,string userId)
    {
        Stock += stock;
        InventoryTransactions ??= new List<InventoryTransaction>();
        InventoryTransactions.Add(new InventoryTransaction(stock, userId, orderId));
    }
}