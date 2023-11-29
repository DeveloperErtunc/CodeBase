namespace CodeBase.Models.DTOModels.BookDTOS;
public class CreateBookDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public int Stock { get; set; }
}
