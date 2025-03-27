using Nop.Core.Caching;

namespace KedemMarket.Fairs.Services;
public class FairCacheSettings
{
    public CacheKey GetCacheKeyByFairName(string name, int customerId)
    {
        return new CacheKey(NopEntityCacheDefaults<Fair>.Prefix, $"{name}-{nameof(customerId)}:{customerId}");
    }
}
