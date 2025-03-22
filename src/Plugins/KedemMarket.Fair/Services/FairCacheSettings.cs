using Nop.Core.Caching;

namespace KedemMarket.Fair.Services;
public class FairCacheSettings
{
    public CacheKey GetCacheKeyByFairName(string name)
    {
        return new CacheKey(NopEntityCacheDefaults<FairInfo>.Prefix, name);
    }
}
