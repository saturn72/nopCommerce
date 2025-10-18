namespace KedemMarket.Brands.Infrastructure.Consumers;
public class BrandUpdatedConsumer : IConsumer<EntityUpdatedEvent<Brand>>
{
    private readonly IStaticCacheManager _staticCacheManager;

    public BrandUpdatedConsumer(IStaticCacheManager staticCacheManager)
    {
        _staticCacheManager = staticCacheManager;
    }
    public async Task HandleEventAsync(EntityUpdatedEvent<Brand> eventMessage)
    {
        var key = CacheSettings.BuildBrandCacheKey(eventMessage.Entity.Id);
        await _staticCacheManager.RemoveAsync(key);
    }
}
