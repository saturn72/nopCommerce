

namespace KedemMarket.Brands.Admin.Factories;

public class BrandModelAdminFactory : IBrandModelAdminFactory
{
    private readonly IBrandService _brandService;
    private readonly BrandsSettings _settings;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IPictureService _pictureService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly MediaSettings _mediaSettings;

    public BrandModelAdminFactory(
        IBrandService brandService,
        BrandsSettings settings,
        IWorkContext workContext,
        IUrlRecordService urlRecordService,
        IPictureService pictureService,
        IStaticCacheManager staticCacheManager,
        MediaSettings mediaSettings)
    {
        _brandService = brandService;
        _settings = settings;
        _workContext = workContext;
        _urlRecordService = urlRecordService;
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

    public async Task<IList<BrandProductAdminModel>> PrepareBrandProductAdminModelsAsync(IList<Brand> brands)
    {
        var tasks = brands.Select(b => PrepareBrandProductAdminModelAsync(null, b));

        await Task.WhenAll(tasks);
        return tasks.Select(t => t.Result).ToList();
    }
}

