namespace CodeBase.Services.Services;
public class OrderService : ServiceBase<Order>, IOrderService
{
    readonly IBookService _bookService;
    readonly IMapper _mapper;
    readonly IMemoryCache _memoryCache;
    private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

    public OrderService(ICurrentUserService userService, CodeBaseInterViewContext context, IBookService bookService, IMapper mapper, IMemoryCache memoryCache) : base(userService, context)
    {
        _bookService = bookService;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }
    public async Task<CommandResult<OrderDTO>> CreateOrderItem(CreateOrderItemDTO model)
    {
        try
        {
            var userId = _userService.GetUserId();
            var order = await GetAllFiltered(x => x.Id == model.OrderId && x.AppUserId == _userService.GetUserId()).Include(x => x.OrderItems).ThenInclude(x => x.Book).Include(x => x.AppUser).AsTracking().FirstOrDefaultAsync();

            if (order == null)
                return CommandResult<OrderDTO>.NotFound();
            var lockResponse =  MemoryCacheHelper.InmemoryLockBook(model.BookId, _memoryCache);
            if (!lockResponse.IsSucceed)
                return lockResponse;

            var orderItem = order?.OrderItems?.FirstOrDefault(x => model.BookId == x.BookId);
            var book = await _bookService.GetFirstTracked(x => x.Id == model.BookId);
            if (book == null)
            {
                MemoryCacheHelper.RemoveCahe(model.BookId,_memoryCache);
                return CommandResult<OrderDTO>.NotFound();
            }
            if (orderItem == null)
            {
                book.UpdateStock(-model.Quantity, order.Id, userId);
                orderItem= order.AddOrderItem(model.BookId, model.Quantity, userId, book.Price);
            }
            else
            {
                var total = orderItem.Quantity - model.Quantity;
                book.UpdateStock(total, order.Id, userId);
                orderItem.Quantity = model.Quantity;
            }
            if (book.Stock < 0)
            {
                MemoryCacheHelper.RemoveCahe(model.BookId, _memoryCache);

                return CommandResult<OrderDTO>.GetFailed("Not enough Stock");
            }
            if (orderItem.Quantity <= 0)
                order.OrderItems.Remove(orderItem);

            var result = await SaveChangeOneTransaction(book, order);
            MemoryCacheHelper.RemoveCahe(model.BookId, _memoryCache);

            return result.IsSucceed ? CommandResult<OrderDTO>.GetSucceed(_mapper.Map<OrderDTO>(order)) : CommandResult<OrderDTO>.GetFailed(result.Message);
        }
        catch (Exception ex)
        {

            return CommandResult<OrderDTO>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
    private async Task<CommandResult<Order>> SaveChangeOneTransaction(Book book, Order order)
    {
        try
        {
            _dbSet.Update(order);
            _context.Set<Book>().Update(book);
            await _context.SaveChangesAsync();
            return CommandResult<Order>.GetSucceed(order);
        }
        catch (Exception ex)
        {
            return CommandResult<Order>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }

    public async Task<CommandResult<PaginationModel<OrderDTO>>> GetAllOrders(PaginationDTO model)
    {
        try
        {

            var orders = GetAllFiltered(x => x.OrderItems != null && x.OrderItems.Count() >0).Include(x => x.OrderItems).ThenInclude(x => x.Book).Include(x => x.AppUser).AsNoTracking();
            if (model.StatDate.HasValue)
                orders = orders.Where(x => x.CreatedDate > model.StatDate.Value);
            if (model.EndDate.HasValue)
                orders = orders.Where(x => x.CreatedDate < model.EndDate.Value);
            if (!string.IsNullOrEmpty(model.UserId))
                orders = orders.Where(x => x.AppUserId == model.UserId);

            var count=  await orders.CountAsync();
            orders = orders.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize);
            var ordersDTO = _mapper.Map<List<OrderDTO>>(orders);
            var result = new PaginationModel<OrderDTO>(ordersDTO, count, model.PageIndex, model.PageSize);
            return CommandResult<PaginationModel<OrderDTO>>.GetSucceed(result);
        }
        catch (Exception ex)
        {
            return CommandResult<PaginationModel<OrderDTO>>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
    public async Task<CommandResult<OrderDTO>> GetOrderById(Guid id)
    {
        try
        {
            var orderDb = await GetAllFiltered(x => x.Id == id && x.AppUserId == GetUserId()).Include(x => x.OrderItems).ThenInclude(x => x.Book).Include(x => x.AppUser).FirstOrDefaultAsync();
            if (orderDb == null)
                return CommandResult<OrderDTO>.NotFound();

            var order = _mapper.Map<OrderDTO>(orderDb);
            return CommandResult<OrderDTO>.GetSucceed(order);
        }
        catch (Exception ex)
        {
            return CommandResult<OrderDTO>.GetFailed(JsonConvert.SerializeObject(ex));
        }
    }
    public async Task<CommandResult<OrderDTO>> CreateOrder()
    {
        var result = await Add(new Order(_userService.GetUserId()));
        return result.IsSucceed ?
          CommandResult<OrderDTO>.GetSucceed(_mapper.Map<OrderDTO>(result.Data)) : CommandResult<OrderDTO>.GetFailed(result.Message);
    }

    public async Task<CommandResult<List<OrderStatistics>>> GetOrderStatistics(string? userId)
    {
        try
        {
            var year = DateTimeOffset.UtcNow.Year;
            var startDatequery = new DateTimeOffset(year, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var data = GetAllFiltered(x => x.CreatedDate >= startDatequery).Include(x => x.AppUser).Include(s => s.OrderItems).ThenInclude(s => s.Book).AsNoTracking();
            if (!string.IsNullOrEmpty(userId))
                data = data.Where(x => x.AppUserId == userId);

            var datas = await data.ToListAsync();
            List<OrderStatistics> statics = new List<OrderStatistics>();
            for (int i = 1; i <= 12; i++)
            {
                var startDate = new DateTimeOffset(year, i, 1, 0, 0, 0, TimeSpan.Zero);
                var Enddate = new DateTimeOffset(year, i, 1, 0, 0, 00, TimeSpan.Zero).AddMonths(1).AddDays(-1);
                var montlyData = datas.Where(x => startDate < x.CreatedDate && Enddate > x.CreatedDate).ToList();
                var orderItems = montlyData.SelectMany(x => x.OrderItems).ToList();
                var staticsmonthly = new OrderStatistics
                {
                    TotalOrderCount = montlyData.Count,
                    Amount = orderItems.Sum(x => x.Price * x.Quantity),
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.Month),
                    TotalBookCount = orderItems.Sum(x => x.Quantity)
                };
                statics.Add(staticsmonthly);
            }
            return CommandResult<List<OrderStatistics>>.GetSucceed(statics);
        }
        catch (Exception ex)
        {

            return CommandResult<List<OrderStatistics>>.GetFailed(JsonConvert.SerializeObject(ex));
        }

    }
}
