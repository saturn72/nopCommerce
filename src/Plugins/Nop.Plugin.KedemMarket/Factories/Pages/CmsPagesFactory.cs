using KedemMarket.Models.Pages.HomePage;
using Nop.Services.Blogs;
using Nop.Services.Logging;

namespace KedemMarket.Factories.Pages;

public class CmsPagesFactory : ICmsPagesFactory
{
    private readonly KedemMarket.Factories.Navbar.INavbarFactory _navbarFactory;
    private readonly IVendorApiModelFactory _vendorFactory;
    private readonly ILogger _logger;
    private readonly ICacheKeyService _cacheKeyService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IBlogModelFactory _blogFactory;
    private readonly IBlogService _blogService;
    private readonly IStoreContext _storeContext;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IWorkContext _workContext;
    private readonly TimeProvider _timeProvider;

    public CmsPagesFactory(
    KedemMarket.Factories.Navbar.INavbarFactory navbarFactory,
        ILogger logger,
        IVendorApiModelFactory vendorFactory,
        ICacheKeyService cacheKeyService,
        IStaticCacheManager staticCacheManager,
        IBlogModelFactory blogFactory,
        IBlogService blogService,
        IStoreContext storeContext,
        IUrlRecordService urlRecordService,
        IWorkContext workContext,
        TimeProvider timeProvider)
    {
        _navbarFactory = navbarFactory;
        _logger = logger;
        _vendorFactory = vendorFactory;
        _cacheKeyService = cacheKeyService;
        _staticCacheManager = staticCacheManager;
        _blogFactory = blogFactory;
        _blogService = blogService;
        _storeContext = storeContext;
        _urlRecordService = urlRecordService;
        _workContext = workContext;
        _timeProvider = timeProvider;
    }

    public async Task<HomePageModel> GetHomePageAsync()
    {
        var key = new CacheKey(PageCacheSettings.HOME_CACHE_KEY);
        _cacheKeyService.PrepareKeyForDefaultCache(key);
        return await _staticCacheManager.GetAsync(key, GetHomePageInternalAsync);
    }

    protected virtual async Task<HomePageModel> GetHomePageInternalAsync()
    {
        var storeId = (await _storeContext.GetCurrentStoreAsync()).Id;
        var languageId = await _workContext.GetWorkingLanguageAsync() is Language lang ? lang.Id : 0;
        var blogs = await GetHomepageBlogsAsync(storeId, languageId);

        var categoryNavbar = await _navbarFactory.PrepareNavbarModelByNameAsync("home-page-category-slider");
        if (categoryNavbar == default)
            await _logger.ErrorAsync("\'home-page-category-slider\' navbar could not be found");

        var vendors = await _vendorFactory.GetAllVendorsAsync();

        var model = new HomePageModel
        {
            Blogs = blogs,
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

    private async Task<IEnumerable<HomePageBlogModel>> GetHomepageBlogsAsync(int storeId, int languageId)
    {
        var utcNow = _timeProvider.GetUtcNow().DateTime;

        var blogs = await _blogService.GetAllBlogPostsAsync(
            storeId: storeId,
            languageId: languageId,
            pageSize: 5,
            dateTo: utcNow);

        // Run all slug fetches in parallel
        var slugTasks = blogs.Select(blog => _urlRecordService.GetSeNameAsync(blog, languageId, false, false)).ToArray();
        var slugs = await Task.WhenAll(slugTasks);

        var res = new HomePageBlogModel[blogs.Count];
        for (var i = 0; i < blogs.Count; i++)
        {
            var blog = blogs[i];
            res[i] = new HomePageBlogModel
            {
                Id = blog.Id,
                Title = blog.Title,
                // Subtitle = blog.Subtitle,
                Short = blog.Body.Length > 200 ? blog.Body[..200] + "..." : blog.Body,
                Slug = slugs[i],
                CreatedOnUtc = blog.CreatedOnUtc,
                //AuthorName = blog.Author, // BlogPost does not have Author property
                // ImageUrl = blog.Image, // Set if available
                MetaKeywords = blog.MetaKeywords,
                MetaDescription = blog.MetaDescription,
                MetaTitle = blog.MetaTitle
            };
        }
        return res;
    }
}
