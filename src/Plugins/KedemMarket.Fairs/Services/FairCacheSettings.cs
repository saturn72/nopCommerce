using Nop.Core.Caching;

namespace KedemMarket.Fairs.Services;
public class FairCacheSettings
{
    public CacheKey GetFairCacheKeyByFairName(string name, int customerId)
    {
        return new CacheKey($"fair-{name}-{nameof(customerId)}:{customerId}", NopEntityCacheDefaults<Fair>.Prefix);
    }
    public CacheKey GetFairVendorMapsCacheKeyByFairId(int fairId)
    {
        return new CacheKey($"fairvendormaps-{nameof(fairId)}:{fairId}", NopEntityCacheDefaults<Fair>.Prefix);
    }

    public CacheKey GetFairVendorProductMapCacheKey(string fairName, int vendorId, bool? isApproved, int pageIndex, int pageSize)
    {
        var ia = isApproved.HasValue ? isApproved.Value.ToString() : string.Empty;

        return new CacheKey(
            $"fairvendorproductmap-{nameof(fairName)}:{fairName}-{nameof(vendorId)}:{nameof(isApproved)}{ia}:{vendorId}.page:{pageIndex}.{nameof(pageSize)}:{pageSize}", NopEntityCacheDefaults<Fair>.Prefix);
    }
}
