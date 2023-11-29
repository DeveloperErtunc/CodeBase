namespace CodeBase.Models.ViewModels;
public class BookLockRedis
{
    public Guid BookId { get; set; }
    public int Stock { get; set; }
    public int ThreadCount { get; set; }
}
