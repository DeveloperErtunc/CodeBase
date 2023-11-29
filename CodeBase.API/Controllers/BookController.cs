namespace CodeBase.API.Controllers;
public class BookController : BaseController
{
    readonly IBookService _bookService;
    public BookController(IMapper mapper, IBookService bookService) : base(mapper)
    {
        _bookService = bookService;
    }
    [HttpPost]
    [Route("GetBooks")]
    public async Task<CommandResult<PaginationModel<BookDTO>>> GetBooks(PaginationDTO request) =>await _bookService.GetBooks(request);
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("Create")]
    public async Task<CommandResult<BookDTO>> Create(CreateBookDTO request) =>await _bookService.CreateBook(request);

    [Authorize(Roles ="Admin")]
    [Route("AddStock")]
    [EnableRateLimiting("CreateOrderPolicy")]
    [HttpPost]
    public async Task<CommandResult> AddStock(InventoryUpdateDTO request) => await _bookService.UpdateStok(request);
}
