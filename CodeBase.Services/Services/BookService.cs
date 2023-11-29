namespace CodeBase.Services.Services;
public class BookService : ServiceBase<Book>, IBookService
{
    readonly IMapper _mapper;
    readonly IMemoryCache _memoryCache;
    public BookService(ICurrentUserService userService, CodeBaseInterViewContext context, IMapper mapper, IMemoryCache memoryCache) : base(userService, context)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
    }
    public async Task<CommandResult<BookDTO>> CreateBook(CreateBookDTO request)
    {
        try
        {
            var book = new Book(request.Name, request.Price, request.Author, request.Stock, _userService.GetUserId());
            var result = await Add(book);
            return result.IsSucceed ? CommandResult<BookDTO>.GetSucceed(_mapper.Map<BookDTO>(book)) :
                CommandResult<BookDTO>.GetFailed(result.Message);
        }
        catch (Exception ex)
        {
            return CommandResult<BookDTO>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
    public async Task<CommandResult<PaginationModel<BookDTO>>> GetBooks(PaginationDTO request)
    {
        try
        {
            var count = await _dbSet.CountAsync();
            var books = Paginate(request.PageIndex, request.PageSize);
            var bookDTO =_mapper.Map<List<BookDTO>>(books);
            var model = new PaginationModel<BookDTO>(bookDTO, count, request.PageIndex, request.PageSize);
            return CommandResult<PaginationModel<BookDTO>>.GetSucceed(model);
        }
        catch (Exception ex)
        {
            return CommandResult<PaginationModel<BookDTO>>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
    public async Task<CommandResult<BookDTO>> UpdateStok(InventoryUpdateDTO request)
    {
        try
        {
            var lockResponse =  MemoryCacheHelper.InmemoryLockBook(request.BookId, _memoryCache);
            if (!lockResponse.IsSucceed)
                return CommandResult<BookDTO>.GetFailed(lockResponse.Message);
                
            var book = await GetFirstTracked(s => request.BookId == s.Id);
            if (book == null)
                return CommandResult<BookDTO>.NotFound();

            book.UpdateStock(request.Quantity, null, _userService.GetUserId());
            var result = await Update(book);
            MemoryCacheHelper.RemoveCahe(request.BookId, _memoryCache);

            return result.IsSucceed ? CommandResult<BookDTO>.GetSucceed(_mapper.Map<BookDTO>(book)) : CommandResult<BookDTO>.GetFailed(result.Message);
        }
        catch (Exception ex)
        {
            return CommandResult<BookDTO>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
}
