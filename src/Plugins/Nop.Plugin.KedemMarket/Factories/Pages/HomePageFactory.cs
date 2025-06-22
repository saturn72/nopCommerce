using Nop.Services.Logging;

namespace KedemMarket.Factories.Pages;

public class HomePageFactory : IHomePageFactory
{
    private readonly KedemMarket.Factories.Navbar.INavbarFactory _navbarFactory;
    private readonly IVendorApiModelFactory _vendorFactory;
    //private readonly ICacheKeyService _cacheKeyService;
    private readonly ILogger _logger;
    //private readonly IStaticCacheManager _staticCacheManager;

    public HomePageFactory(
    KedemMarket.Factories.Navbar.INavbarFactory navbarFactory,
        ILogger logger,
        IVendorApiModelFactory vendorFactory)
    //ICacheKeyService cacheKeyService,
    //IStaticCacheManager staticCacheManager)
    {
        _navbarFactory = navbarFactory;
        _logger = logger;
        _vendorFactory = vendorFactory;
        //_cacheKeyService = cacheKeyService;
        //_staticCacheManager = staticCacheManager;
    }

    public async Task<HomePageModel> GetHomePageAsync()
    {
        return await GetHomePageInternalAsync();
        //var ck = new CacheKey(PageCacheSettings.HOME_CACHE_KEY, PageCacheSettings.CACHE_KEY_PREFIX);
        //_cacheKeyService.PrepareKeyForDefaultCache(ck);
        //return await _staticCacheManager.Get(ck, GetHomePageInternalAsync);
    }

    protected virtual async Task<HomePageModel> GetHomePageInternalAsync()
    {
        var categoryNavbar = await _navbarFactory.PrepareNavbarModelByNameAsync("home-page-category-slider");
        if (categoryNavbar == default)
            await _logger.ErrorAsync("\'home-page-category-slider\' navbar could not be found");

        var vendors = await _vendorFactory.GetAllVendorsAsync();

        var model = new HomePageModel
        {
            Categories = categoryNavbar?.Elements ?? [],
            Vendors = vendors,
            //Slides = 
            //    new List<SlideModel>
            //    {
            //        new() {
            //            ImageUrl = "https://image.freepik.com/free-photo/river-foggy-mountains-landscape_1204-510.jpg",
            //            Alt = "Welcome to Kedem Market", },
            //     new() {
            //            ImageUrl = "https://image.freepik.com/free-photo/river-foggy-mountains-landscape_1204-511.jpg",
            //         Alt = "Quality Products" },
            //     new() {
            //            ImageUrl = "https://image.freepik.com/free-photo/river-foggy-mountains-landscape_1204-512.jpg",
            //         Alt = "Fast Delivery" },
            //     new() {
            //            ImageUrl = "https://image.freepik.com/free-photo/river-foggy-mountains-landscape_1204-513.jpg",
            //         Alt = "Customer Satisfaction" }
            //}
        };
        return model;
    }
}
