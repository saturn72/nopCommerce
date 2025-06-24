namespace KedemMarket.Factories.Pages;

public class EntityToModelFactory : IEntityToModelFactory
{
    private readonly Dictionary<string, Func<int, Task<object>>> _handlers;
    private IProductService _productService;
    private readonly IStoreContext _storeContext;
    private readonly IStoreMappingService _storeMappingService;
    private readonly IProductApiFactory _productApiFactory;
    private readonly IVendorService _vendorService;
    private readonly IVendorApiModelFactory _vendorApiModelFactory;

    public EntityToModelFactory(
        IProductService productService,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        IProductApiFactory productApiFactory,
        IVendorService vendorService,
        IVendorApiModelFactory vendorApiModelFactory)
    {
        _handlers = new Dictionary<string, Func<int, Task<object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(Product), GetProductById },
            { nameof(Vendor), GetVendorById}
        };
        _productService = productService;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
        _productApiFactory = productApiFactory;
        _vendorService = vendorService;
        _vendorApiModelFactory = vendorApiModelFactory;
    }

    public async Task<object> GetModelEntityByTypeNameAndEntityId(string entityName, int entityId)
    {
        if (!_handlers.TryGetValue(entityName, out var h) || h == null)
            return null;
        return await h(entityId);
    }

    protected virtual async Task<object> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null || product.Deleted)
            return null;

        if (product.LimitedToStores)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var storeMap = await _storeMappingService.GetStoreMappingsAsync(product);
            if (storeMap.All(s => s.StoreId != store.Id))
                return null;
        }

        var all = await _productApiFactory.ToProductInfoApiModelAsync([product]);
        return all?.FirstOrDefault();
    }

    protected virtual async Task<object> GetVendorById(int id)
    {
        var vendor = await _vendorService.GetVendorByIdAsync(id);
        if (vendor == default)
            return null;

        return await _vendorApiModelFactory.PrepareVendorApiModelAsync(vendor);
    }
}
