namespace CodeBase.Helpers;
public static class MemoryCacheHelper
{
    private readonly static string lockBookKey = $"bookLock";
    public static CommandResult<OrderDTO> InmemoryLockBook(Guid bookId, IMemoryCache memoryCache)
    {

        var key = $"{lockBookKey}_{bookId}";
        if (memoryCache.TryGetValue(key, out _))
        {
            return CommandResult<OrderDTO>.GetFailed($"Bu kitap şu anda başka kullanıcı tarafından işlem görüyor lütfen bir kaç saniye sonra deniniyiz");
        }
        else
        {
            memoryCache.Set(key, true, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(40) // Örneğin, 10 saniye boyunca kitabı işleme kitle
            });
        }
        return CommandResult<OrderDTO>.GetSucceed(null);
    }
    public static void RemoveCahe(Guid bookId, IMemoryCache memoryCache)
    {
        var key = $"{lockBookKey}_{bookId}";
        memoryCache.Remove(key);
    }
}
