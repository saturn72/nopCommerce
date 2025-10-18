
namespace KedemMarket.Brands.Factories;

public class BrandModelFactory : IBrandModelFactory
{
    private readonly IBrandService _brandService;
    private readonly BrandsSettings _settings;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IThumbService _thumbService;
    private readonly IPictureService _pictureService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly MediaSettings _mediaSettings;

    public BrandModelFactory(
        IBrandService brandService,
        BrandsSettings settings,
        IWorkContext workContext,
        IUrlRecordService urlRecordService,
        IThumbService thumbService,
        IPictureService pictureService,
        IStaticCacheManager staticCacheManager,
        MediaSettings mediaSettings)
    {
        _brandService = brandService;
        _settings = settings;
        _workContext = workContext;
        _urlRecordService = urlRecordService;
        _thumbService = thumbService;
        _pictureService = pictureService;
        _staticCacheManager = staticCacheManager;
        _mediaSettings = mediaSettings;
    }

    public async Task<BrandSearchModel> PrepareBrandSearchModelAsync(BrandSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var vendor = await _workContext.GetCurrentVendorAsync();
        searchModel.IsLoggedInAsVendor = vendor != null;
        searchModel.AllowVendorsToImportBrands = _settings.AllowVendorsToImportBrands;
        searchModel.SetGridPageSize();

        return searchModel;
    }

    public async Task<BrandListModel> PrepareBrandListModelAsync(BrandSearchModel searchModel)
    {
        searchModel ??= new();

        var brands = await _brandService.GetAllBrandsAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize,
            names: [searchModel.SearchBrandName]);

        return await new BrandListModel().PrepareToGridAsync(
            searchModel,
            brands,
             () => brands.SelectAwait(async b => await PrepareBrandProductAdminModelAsync(null, b))
        );
    }

    public async Task<BrandProductAdminModel> PrepareBrandProductAdminModelAsync(BrandProductAdminModel brandModel, Brand brand)
    {
        ThrowIfNull(brand);

        var logoTask = _pictureService.GetPictureUrlAsync(brand.LogoImageId);
        var seNameTask = _urlRecordService.GetSeNameAsync(brand);

        await Task.WhenAll(logoTask, seNameTask);

        return (brandModel ?? new BrandProductAdminModel()) with
        {
            Id = brand.Id,
            Comment = brand.Comment,
            DisplayOrder = brand.DisplayOrder,
            LogoUrl = logoTask.Result,
            Name = brand.Name,
            SeName = seNameTask.Result
        };
    }

    public async Task<IEnumerable<BrandProductAdminModel>> PrepareBrandProductAdminModelsAsync(IList<Brand> brands)
    {
        var tasks = brands.Select(b => PrepareBrandProductAdminModelAsync(null, b));

        await Task.WhenAll(tasks);
        return tasks.Select(t => t.Result);
    }

    public async Task<IEnumerable<BrandProductModel>> PrepareProductBrandProductModelsAsync(int productId)
    {
        var brands = await _brandService.GetProductBrandsByProductIdAsync(productId);
        if (brands == null || brands.Count <= 0)
            return Array.Empty<BrandProductModel>();

        var tasks = brands.Select(async brand =>
        {
            var logoTask = GetBrandLogoAsync(brand);
            var seNameTask = _urlRecordService.GetSeNameAsync(brand);

            await Task.WhenAll(logoTask, seNameTask);

            return new BrandProductModel
            {
                Id = brand.Id,
                DisplayOrder = brand.DisplayOrder,
                Logo = logoTask.Result,
                Name = brand.Name,
                SeName = seNameTask.Result
            };
        });
        await Task.WhenAll(tasks);
        return tasks.
            Select(t => t.Result)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(f => f.Name);

    }
    protected virtual async Task<PictureModel?> GetBrandLogoAsync(Brand brand)
    {
        var cacheKey = CacheSettings.BuildBrandCacheKey(brand.Id);
        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var logo = await _pictureService.GetPictureByIdAsync(brand.LogoImageId);
            if (logo == null)
                return null;

            var (thumbImageUrl, picture) = await _pictureService.GetPictureUrlAsync(
                logo,
                _mediaSettings.ProductThumbPictureSizeOnProductDetailsPage,
                defaultPictureType: PictureType.Avatar);

            var pictureModel = new PictureModel
            {
                Id = picture.Id,
                ThumbImageUrl = thumbImageUrl,
                Title = picture.TitleAttribute,
                AlternateText = picture.AltAttribute
            };
            return pictureModel;
        });
    }
}

