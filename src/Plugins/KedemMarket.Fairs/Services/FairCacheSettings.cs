using Nop.Core.Caching;

namespace KedemMarket.Fairs.Services;
public class FairCacheSettings
{
    public CacheKey GetFairCacheKeyByFairName(string name, int customerId)
    {
        return new CacheKey(NopEntityCacheDefaults<Fair>.Prefix, $"fair-{name}-{nameof(customerId)}:{customerId}");
    }
    public CacheKey GetFairVendorMapsCacheKeyByFairId(int fairId)
    {
        return new CacheKey(NopEntityCacheDefaults<Fair>.Prefix, $"fairvendormaps-{nameof(fairId)}:{fairId}");
    }

    public CacheKey GetFairVendorProductMapCacheKey(string fairName, int vendorId, int pageIndex, int pageSize)
    {
        return new CacheKey(NopEntityCacheDefaults<Fair>.Prefix, $"fairvendorproductmap-{nameof(fairName)}:{fairName}-{nameof(vendorId)}:{vendorId}.page:{pageIndex}.{nameof(pageSize)}:{pageSize}");
    }
}
