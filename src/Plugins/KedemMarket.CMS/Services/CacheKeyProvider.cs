
using System.Collections.Concurrent;

namespace KedemMarket.CMS.Services;

public class CacheKeyProvider : ICacheKeyProvider
{
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ConcurrentDictionary<string, CacheKey> _cacheKeys;
    public CacheKeyProvider(ICacheKeyService cacheKeyService)
    {
        _cacheKeyService = cacheKeyService;
        _cacheKeys = new ConcurrentDictionary<string, CacheKey>(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<CacheKey> GetCacheKeyAsync(string key)
    {
        if (!_cacheKeys.TryGetValue(key, out var value))
        {
            value = new CacheKey(key);
            //var ck = _cacheKeyService.PrepareKeyForDefaultCache(value);
            _cacheKeys.TryAdd(key, value);
            //value = ck;
        }
        return value;
    }
}
