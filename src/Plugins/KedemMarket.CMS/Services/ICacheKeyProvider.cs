
namespace KedemMarket.CMS.Services;
public interface ICacheKeyProvider
{
    public Task<CacheKey> GetCacheKeyAsync(string key);
}
