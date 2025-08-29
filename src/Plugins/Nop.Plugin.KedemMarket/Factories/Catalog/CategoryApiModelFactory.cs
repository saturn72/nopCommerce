using System.Runtime.ConstrainedExecution;

namespace KedemMarket.Factories.Catalog;

public class CategoryApiModelFactory : ICategoryApiModelFactory
{
    private readonly ICatalogModelFactory _catalogModelFactory;
    private readonly IMediaManager _mediaConverter;
    private readonly IProductApiFactory _productApiFactory;
    private readonly IProductService _productService;
    private readonly IStoreContext _storeContext;
    private readonly ICategoryService _categoryService;

    public CategoryApiModelFactory(
        ICatalogModelFactory catalogModelFactory,
        IMediaManager mediaConverter,
        IProductApiFactory productApiFactory,
        IProductService productService,
        IStoreContext storeContext,
        ICategoryService categoryService)
    {
        _catalogModelFactory = catalogModelFactory;
        _mediaConverter = mediaConverter;
        _productApiFactory = productApiFactory;
        _productService = productService;
        _storeContext = storeContext;
        _categoryService = categoryService;
    }

    private async Task<string?> GetCategoryThumbImageAsync(int categoryId)
    {
        var category = await _categoryService.GetCategoryByIdAsync(categoryId);
        return category.PictureId > 0 ?
        await _mediaConverter.GetDownloadLinkAsync(category.PictureId, KmConsts.MediaTypes.Thumbnail)
        : default;
    }

    public async Task<CategoryApiModel> PrepareCategoryApiModelAsync(Category category)
    {
        var model = await _catalogModelFactory.PrepareCategoryModelAsync(category, new CatalogProductsCommand());
        var breadcrumbs = new List<CategorySlimApiModel>();
        if (model.CategoryBreadcrumb.NotNullAndNotNotEmpty())
        {
            for (var i = 0; i < model.CategoryBreadcrumb.Count; i++)
            {
                var cur = model.CategoryBreadcrumb.ElementAt(i);
                breadcrumbs.Add(new()
                {
                    Id = cur.Id,
                    Name = cur.Name,
                    Slug = cur.SeName,
                    Description = cur.Description,
                    ThumbnailUrl = await GetCategoryThumbImageAsync(cur.Id),
                });
            }
        }

        var subCategories = new List<CategorySlimApiModel>();
        if (model.SubCategories != null && model.SubCategories.Count > 0)
        {
            foreach (var sc in model.SubCategories)
                subCategories.Add(new CategorySlimApiModel
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Slug = sc.SeName,
                    Description = sc.Description,
                    ThumbnailUrl = await GetCategoryThumbImageAsync(sc.Id),
                });
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
            categoryIds: [category.Id],
            excludeFeaturedProducts: true,
            storeId: store.Id,
            orderBy: ProductSortingEnum.Position);

        var products = await _productApiFactory.ToProductInfoApiModelAsync(ps);

        var (imageUrl, thumbUrl) = category.PictureId > 0 ?
        (await _mediaConverter.GetDownloadLinkAsync(category.PictureId, KmConsts.MediaTypes.Image),
        await _mediaConverter.GetDownloadLinkAsync(category.PictureId, KmConsts.MediaTypes.Thumbnail))
        : (default, default);

        return new CategoryApiModel
        {
            Id = model.Id,
            Breadcrumbs = breadcrumbs,
            Description = model.Description,
            FeaturedProducts = featuredProducts,
            ImageUrl = imageUrl,
            JsonLd = model.JsonLd,
            MetaKeywords = model.MetaKeywords,
            MetaDescription = model.MetaDescription,
            MetaTitle = model.MetaTitle,
            Name = model.Name,
            Products = products,
            ThumbnailUrl = thumbUrl,
            Slug = model.SeName,
            SubCategories = subCategories,
        };
    }
}
