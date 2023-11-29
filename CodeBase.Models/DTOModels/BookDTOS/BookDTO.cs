namespace CodeBase.Models.DTOModels.BookDTOS;
public class BookDTO: BaseDTO
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Author { get; set; }
    public int Stock { get; set; }
}
