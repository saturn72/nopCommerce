using Nop.Core.Caching;
using Nop.Core.Events;

namespace KedemMarket.Fairs.Consumers;
public class ClearCacheConsumer :
    IConsumer<EntityInsertedEvent<Fair>>,
    IConsumer<EntityUpdatedEvent<Fair>>,
    IConsumer<EntityDeletedEvent<Fair>>,

    IConsumer<EntityInsertedEvent<FairVendorMap>>,
    IConsumer<EntityUpdatedEvent<FairVendorMap>>,
    IConsumer<EntityDeletedEvent<FairVendorMap>>,

    IConsumer<EntityInsertedEvent<FairAddressMap>>,
    IConsumer<EntityUpdatedEvent<FairAddressMap>>,
    IConsumer<EntityDeletedEvent<FairAddressMap>>
{
    private readonly IShortTermCacheManager _shortTermCacheManager;
    private readonly IStaticCacheManager _staticCacheManager;

    public ClearCacheConsumer(
        IShortTermCacheManager shortTermCacheManager,
        IStaticCacheManager staticCacheManager)
    {
        _shortTermCacheManager = shortTermCacheManager;
        _staticCacheManager = staticCacheManager;
    }

    public Task HandleEventAsync(EntityInsertedEvent<Fair> eventMessage) => ClearFairCacheByPrefixAsync();
    public Task HandleEventAsync(EntityUpdatedEvent<Fair> eventMessage) => ClearFairCacheByPrefixAsync();
    public Task HandleEventAsync(EntityDeletedEvent<Fair> eventMessage) => ClearFairCacheByPrefixAsync();

    public Task HandleEventAsync(EntityInsertedEvent<FairVendorMap> eventMessage) => ClearFairCacheByPrefixAsync();
    public Task HandleEventAsync(EntityUpdatedEvent<FairVendorMap> eventMessage) => ClearFairCacheByPrefixAsync();
    public Task HandleEventAsync(EntityDeletedEvent<FairVendorMap> eventMessage) => ClearFairCacheByPrefixAsync();

    public Task HandleEventAsync(EntityInsertedEvent<FairAddressMap> eventMessage) => ClearFairCacheByPrefixAsync();
    public Task HandleEventAsync(EntityUpdatedEvent<FairAddressMap> eventMessage) => ClearFairCacheByPrefixAsync();
    public Task HandleEventAsync(EntityDeletedEvent<FairAddressMap> eventMessage) => ClearFairCacheByPrefixAsync();

    private async Task ClearFairCacheByPrefixAsync()
    {
        var key = NopEntityCacheDefaults<Fair>.Prefix;
        _shortTermCacheManager.RemoveByPrefix(key);
        await _staticCacheManager.RemoveByPrefixAsync(key);
    }
}
