using KedemMarket.Cms.Models.Pages.HomePage;
using KedemMarket.CMS.Services;

namespace KedemMarket.Cms.Factories.Pages;

public class CmsPagesFactory : ICmsPagesFactory
{
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IProductService _productService;
    private readonly IProductModelFactory _productModelFactory;
    private readonly ICatalogModelFactory _catalogModelFactory;
    private readonly ICacheKeyProvider _cacheKeyProvider;

    public CmsPagesFactory(
        IStaticCacheManager staticCacheManager,
        IProductService productService,
        IProductModelFactory productModelFactory,
        ICatalogModelFactory catalogModelFactory,
        ICacheKeyProvider cacheKeyProvider)
    {
        _staticCacheManager = staticCacheManager;
        _productService = productService;
        _productModelFactory = productModelFactory;
        _catalogModelFactory = catalogModelFactory;
        _cacheKeyProvider = cacheKeyProvider;
    }

    public async Task<HomePageModel> GetHomePageAsync()
    {
        var ck = await _cacheKeyProvider.GetCacheKeyAsync(PageCacheSettings.HOME_CACHE_KEY);
        return await _staticCacheManager.GetAsync(ck, GetHomePageInternalAsync);
    }

    protected virtual async Task<HomePageModel> GetHomePageInternalAsync()
    {
        var storeProducts = await (await _productService.GetAllProductsDisplayedOnHomepageAsync())
          //ACL and store mapping
          //.WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
          //availability dates
          .Where(p => _productService.ProductIsAvailable(p))
          //visible individually
          .Where(p => p.VisibleIndividually).ToListAsync();

        var products = storeProducts?.Any() == true ?
            (await _productModelFactory.PrepareProductOverviewModelsAsync(storeProducts, true, true, null)).ToList() :
            [];
        var categories = await _catalogModelFactory.PrepareHomepageCategoryModelsAsync();
        var model = new HomePageModel
        {
            Categories = categories,
            Products = products,
            //Vendors = vendors,
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

    //private async Task<IEnumerable<HomePageBlogModel>> GetHomepageBlogsAsync(int storeId, int languageId)
    //{
    //    var utcNow = _timeProvider.GetUtcNow().DateTime;

    //    var blogs = await _blogService.GetAllBlogPostsAsync(
    //        storeId: storeId,
    //        languageId: languageId,
    //        pageSize: 5,
    //        dateTo: utcNow);

    //    // Run all slug fetches in parallel
    //    var slugTasks = blogs.Select(blog => _urlRecordService.GetSeNameAsync(blog, languageId, false, false)).ToArray();
    //    var slugs = await Task.WhenAll(slugTasks);

    //    var res = new HomePageBlogModel[blogs.Count];
    //    for (var i = 0; i < blogs.Count; i++)
    //    {
    //        var blog = blogs[i];
    //        res[i] = new HomePageBlogModel
    //        {
    //            Id = blog.Id,
    //            Title = blog.Title,
    //            // Subtitle = blog.Subtitle,
    //            Short = blog.Body.Length > 200 ? blog.Body[..200] + "..." : blog.Body,
    //            Slug = slugs[i],
    //            CreatedOnUtc = blog.CreatedOnUtc,
    //            //AuthorName = blog.Author, // BlogPost does not have Author property
    //            // ImageUrl = blog.Image, // Set if available
    //            MetaKeywords = blog.MetaKeywords,
    //            MetaDescription = blog.MetaDescription,
    //            MetaTitle = blog.MetaTitle
    //        };
    //    }
    //    return res;
    //}
}
