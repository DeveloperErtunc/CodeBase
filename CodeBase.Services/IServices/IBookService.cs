namespace CodeBase.Services.IServices;
public interface IBookService:IServiceBase<Book>
{
    Task<CommandResult<BookDTO>> CreateBook(CreateBookDTO request);
    Task<CommandResult<BookDTO>> UpdateStok(InventoryUpdateDTO request);
    Task<CommandResult<PaginationModel<BookDTO>>> GetBooks(PaginationDTO request);
}
