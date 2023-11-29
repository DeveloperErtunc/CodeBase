namespace CodeBase.Services.IServices;
public interface IServiceBase<T> where T : BaseEntity
{
    Task<int> CountAsync();
    IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? expression = null);
    Task<T> GetFirst(Expression<Func<T, bool>> expression);
    Task<T> GetFirstTracked(Expression<Func<T, bool>> expression);
    Task<CommandResult<T>> Update(T entity);
    Task<CommandResult<List<T>>> UpdateMany(List<T> entities);
    Task<CommandResult<T>> Add(T entity);
    Task<CommandResult<T>> AddorUpdate(T entity);
    Task<CommandResult<T>> DeleteAndFind(Guid id);
    Task<CommandResult<T>> Delete(T entity);
    Task<CommandResult<List<T>>> DeleteMany(List<T> models);
    Task<CommandResult<List<T>>> AddMany(List<T> entities);
    IQueryable<T> Paginate(int PageIndex, int PageSize, Expression<Func<T, bool>> expression = null);
}