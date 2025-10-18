namespace KedemMarket.Brands.Infrastructure;

public class CacheSettings
{
    public const string BRAND_CACHE_KEY_FORMAT = "km.brand.{0}";

    public static CacheKey BuildBrandCacheKey(int brandId)
    {
            var ck = string.Format(BRAND_CACHE_KEY_FORMAT, brandId);
        return new CacheKey(ck);
    }
}