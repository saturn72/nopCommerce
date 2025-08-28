namespace KedemMarket.Factories.Catalog;

public class CategoryApiModelFactory : ICategoryApiModelFactory
{
    private readonly ICatalogModelFactory _catalogModelFactory;
    private readonly IMediaManager _mediaConverter;
    private readonly IProductApiFactory _productApiFactory;
    private readonly IProductService _productService;
    private readonly IStoreContext _storeContext;

    public CategoryApiModelFactory(
        ICatalogModelFactory catalogModelFactory,
        IMediaManager mediaConverter,
        IProductApiFactory productApiFactory,
        IProductService productService,
        IStoreContext storeContext)
    {
        _catalogModelFactory = catalogModelFactory;
        _mediaConverter = mediaConverter;
        _productApiFactory = productApiFactory;
        _productService = productService;
        _storeContext = storeContext;
    }
    public async Task<CategoryApiModel> PrepareCategoryApiModelAsync(Category category)
    {
        var model = await _catalogModelFactory.PrepareCategoryModelAsync(category, new CatalogProductsCommand());

        var subCategories = new List<CategorySlimApiModel>();
        if (model.SubCategories != null && model.SubCategories.Count > 0)
        {
            foreach (var sc in model.SubCategories)
            {
                subCategories.Add(new CategorySlimApiModel
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Slug = sc.SeName,
                    Description = sc.Description,
                    ThumbnailUrl = await _mediaConverter.GetDownloadLinkAsync(category.PictureId, KmConsts.MediaTypes.Thumbnail),
                });
            }
        }

        var featuredProducts = Enumerable.Empty<ProductSlimApiModel>();
        if (model.FeaturedProducts != null && model.FeaturedProducts.Count > 0)
        {
            var featuredProductIds = model.FeaturedProducts.Select(fp => fp.Id).ToArray();
            var fps = await _productService.GetProductsByIdsAsync(featuredProductIds);
            featuredProducts = await _productApiFactory.ToProductSlimApiModelAsync(fps);
        }

        var store = await _storeContext.GetCurrentStoreAsync();
        var ps = await _productService.SearchProductsAsync(
            0,
            10,
            categoryIds: [category.Id] ,
            excludeFeaturedProducts: true,
            storeId: store.Id,
            orderBy: ProductSortingEnum.Position);

        var products = await _productApiFactory.ToProductInfoApiModelAsync(ps);

        return new CategoryApiModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            MetaKeywords = model.MetaKeywords,
            MetaDescription = model.MetaDescription,
            MetaTitle = model.MetaTitle,
            ImageUrl = await _mediaConverter.GetDownloadLinkAsync(category.PictureId, KmConsts.MediaTypes.Image),
            ThumbnailUrl = await _mediaConverter.GetDownloadLinkAsync(category.PictureId, KmConsts.MediaTypes.Thumbnail),
            Slug = model.SeName,
            SubCategories = subCategories,
            FeaturedProducts = featuredProducts,
            Products = products,
            JsonLd = model.JsonLd,
        };
    }
}
