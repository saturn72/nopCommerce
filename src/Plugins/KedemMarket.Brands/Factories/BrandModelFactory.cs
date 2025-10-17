using Nop.Services.Media;
using Nop.Web.Framework.Models.Extensions;

namespace KedemMarket.Brands.Factories;

public class BrandModelFactory : IBrandModelFactory
{
    private readonly IBrandService _brandService;
    private readonly BrandsSettings _settings;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IThumbService _thumbService;
    private readonly IPictureService _pictureService;

    public BrandModelFactory(
        IBrandService brandService,
        BrandsSettings settings,
        IWorkContext workContext,
        IUrlRecordService urlRecordService,
        IThumbService thumbService,
        IPictureService pictureService)
    {
        _brandService = brandService;
        _settings = settings;
        _workContext = workContext;
        _urlRecordService = urlRecordService;
        _thumbService = thumbService;
        _pictureService = pictureService;
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
             () => brands.SelectAwait(async b => await PrepareBrandModelAsync(null, b))
        );
    }

    public async Task<BrandModel> PrepareBrandModelAsync(BrandModel brandModel, Brand brand)
    {
        if (brand == null)
            return brandModel ?? new BrandModel();

        var logoTask = _pictureService.GetPictureUrlAsync(brand.LogoImageId);
        var seNameTask = _urlRecordService.GetSeNameAsync(brand);

        await Task.WhenAll(logoTask, seNameTask);

        return (brandModel ?? new BrandModel()) with
        {
            Id = brand.Id,
            Comment = brand.Comment,
            DisplayOrder = brand.DisplayOrder,
            LogoUrl = logoTask.Result,
            Name = brand.Name,
            SeName = seNameTask.Result
        };
    }
}

