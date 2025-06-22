namespace KedemMarket.Consumers;
public class ClearNavbarCacheConsumer :
    IConsumer<EntityUpdatedEvent<Vendor>>,
    IConsumer<EntityDeletedEvent<Vendor>>,

    IConsumer<EntityDeletedEvent<VendorAttributeValue>>,
    IConsumer<EntityInsertedEvent<VendorAttributeValue>>,
    IConsumer<EntityUpdatedEvent<VendorAttributeValue>>,

    IConsumer<EntityInsertedEvent<NavbarInfo>>,
    IConsumer<EntityUpdatedEvent<NavbarInfo>>,
    IConsumer<EntityDeletedEvent<NavbarInfo>>,

    IConsumer<EntityInsertedEvent<NavbarElement>>,
    IConsumer<EntityUpdatedEvent<NavbarElement>>,
    IConsumer<EntityDeletedEvent<NavbarElement>>,

    IConsumer<EntityInsertedEvent<NavbarElementVendor>>,
    IConsumer<EntityUpdatedEvent<NavbarElementVendor>>,
    IConsumer<EntityDeletedEvent<NavbarElementVendor>>,

    IConsumer<EntityInsertedEvent<Product>>,
    IConsumer<EntityUpdatedEvent<Product>>,
    IConsumer<EntityDeletedEvent<Product>>,

    IConsumer<EntityInsertedEvent<Category>>,
    IConsumer<EntityUpdatedEvent<Category>>,
    IConsumer<EntityDeletedEvent<Category>>

{
    private readonly INavbarService _navbarService;
    private IStaticCacheManager _staticCacheManager;

    public ClearNavbarCacheConsumer(
        INavbarService navbarService,
        IStaticCacheManager staticCacheManager)
    {
        _navbarService = navbarService;
        _staticCacheManager = staticCacheManager;
    }
    public async Task HandleEventAsync(EntityUpdatedEvent<Vendor> eventMessage) =>
        await ClearNavbarVendorCacheAsync(eventMessage.Entity?.Id);

    public async Task HandleEventAsync(EntityDeletedEvent<Vendor> eventMessage) =>
        await ClearNavbarVendorCacheAsync(eventMessage.Entity?.Id);

    public async Task HandleEventAsync(EntityDeletedEvent<VendorAttributeValue> eventMessage) =>
        await _staticCacheManager.RemoveByPrefixAsync(NavbarCacheSettings.CACHE_KEY);

    public async Task HandleEventAsync(EntityInsertedEvent<VendorAttributeValue> eventMessage) =>
        await _staticCacheManager.RemoveByPrefixAsync(NavbarCacheSettings.CACHE_KEY);

    public async Task HandleEventAsync(EntityUpdatedEvent<VendorAttributeValue> eventMessage) =>
        await _staticCacheManager.RemoveByPrefixAsync(NavbarCacheSettings.CACHE_KEY);

    private async Task ClearNavbarVendorCacheAsync(int? vendorId)
    {
        if (vendorId <= 0)
            return;

        await _staticCacheManager.RemoveByPrefixAsync(NavbarCacheSettings.CACHE_KEY);
    }
    public async Task HandleEventAsync(EntityInsertedEvent<NavbarInfo> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityUpdatedEvent<NavbarInfo> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);

    public async Task HandleEventAsync(EntityDeletedEvent<NavbarInfo> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);

    public async Task HandleEventAsync(EntityInsertedEvent<NavbarElement> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityUpdatedEvent<NavbarElement> eventMessage) =>
       await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityDeletedEvent<NavbarElement> eventMessage) =>
       await ClearRelevantCacheAsync(eventMessage.Entity);

    public async Task HandleEventAsync(EntityInsertedEvent<NavbarElementVendor> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityUpdatedEvent<NavbarElementVendor> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityDeletedEvent<NavbarElementVendor> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);

    public async Task HandleEventAsync(EntityInsertedEvent<Product> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityUpdatedEvent<Product> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityDeletedEvent<Product> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);

    public async Task HandleEventAsync(EntityInsertedEvent<Category> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityUpdatedEvent<Category> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);
    public async Task HandleEventAsync(EntityDeletedEvent<Category> eventMessage) =>
        await ClearRelevantCacheAsync(eventMessage.Entity);

    private async Task ClearRelevantCacheAsync(BaseEntity entity)
    {
        if (entity == null)
            return;

        await _staticCacheManager.RemoveByPrefixAsync(NavbarCacheSettings.CACHE_KEY);
        await _staticCacheManager.RemoveByPrefixAsync(PageCacheSettings.CACHE_KEY_PREFIX);
    }
}
