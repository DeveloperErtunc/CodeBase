namespace CodeBase.Services.Services;
public class ServiceBase<T> : IServiceBase<T> where T : BaseEntity
{
    protected readonly ICurrentUserService _userService;
    protected readonly CodeBaseInterViewContext _context;
    protected readonly DbSet<T> _dbSet;
    public ServiceBase(ICurrentUserService userService, CodeBaseInterViewContext context)
    {
        _userService = userService;
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }
    public string GetUserId() => _userService.GetUserId();
    public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();
    public async Task<CommandResult<T>> AddorUpdate(T entity) => entity.Id == Guid.Empty ? await Add(entity) : await Update(entity);

    public async Task<CommandResult<T>> Add(T entity)
    {
        entity.Created(GetUserId());
        _context.Set<T>().Add(entity);
        return await SaveAsync(entity);
    }
    public async Task<CommandResult<List<T>>> AddMany(List<T> entities)
    {
        entities.ForEach(x => x.Created(GetUserId()));
        _context.Set<T>().AddRange(entities);
        return await SaveManyAsync(entities);
    }
    public async Task<CommandResult<T>> DeleteAndFind(Guid id)
    {
        var model = await GetFirstTracked(x => x.Id == id);
        if (model == null)
            return CommandResult<T>.NotFound();

        _dbSet.Remove(model);
        return await SaveAsync(model);
    }
    public async Task<CommandResult<List<T>>> DeleteMany(List<T> models)
    {
        _dbSet.RemoveRange(models);
        return await SaveManyAsync(models);
    }
    public async Task<CommandResult<T>> Delete(T entity)
    {
        _dbSet.Remove(entity);
        return await SaveAsync(entity);
    }
    public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? expression = null)
    {
        var result = _dbSet.OrderByDescending(s => s.UpdateDate).AsNoTracking();
        if (expression != null)
            result = result.Where(expression);
        return result;
    }
    public async Task<T> GetFirst(Expression<Func<T, bool>> expression)
    {
        var data = await _dbSet.AsNoTracking().FirstOrDefaultAsync(expression);
        if (data == null) return null;
        return data;
    }
    public async Task<T> GetFirstTracked(Expression<Func<T, bool>> expression)
    {
        var data = await _dbSet.FirstOrDefaultAsync(expression);
        if (data == null) return null;

        return data;
    }
    public async Task<CommandResult<T>> Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
        _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        entity.Updated(GetUserId());

        return await SaveAsync(entity);
    }
    public async Task<CommandResult<List<T>>> UpdateMany(List<T> entities)
    {
        foreach (var entity in entities)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
            _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            entity.Updated(GetUserId());
        }
        return await SaveManyAsync(entities);
    }
    public async Task<CommandResult<List<T>>> SaveManyAsync(List<T> data)
    {
        try
        {
            await _context.SaveChangesAsync();
            return CommandResult<List<T>>.GetSucceed(data);
        }
        catch (Exception ex)
        {
            return CommandResult<List<T>>.GetFailed("Exception Message" + ex?.Message + "Inner Exceptiın" + ex?.InnerException?.Message);
        }
    }
    private async Task<CommandResult<T>> SaveAsync(T data)
    {
        try
        {
            await _context.SaveChangesAsync();
            return CommandResult<T>.GetSucceed(data);
        }
        catch (Exception ex)
        {
            return CommandResult<T>.GetFailed("Exception Message" + ex?.Message + "Inner Exceptiın" + ex?.InnerException?.Message);
        }
    }
    public  IQueryable<T> Paginate(int PageIndex, int PageSize, Expression<Func<T, bool>> expression = null)
    {
        var iq =GetAllFiltered();
        if (expression != null)
            iq = iq.Where(expression);

      return  _dbSet.Skip((PageIndex - 1) * PageSize).Take(PageSize);
    }
}
