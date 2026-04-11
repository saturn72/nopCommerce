namespace KedemMarket.Brands.Factories;

public class BrandModelFactory : IBrandModelFactory
{
    private readonly IBrandService _brandService;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IPictureService _pictureService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly MediaSettings _mediaSettings;

    public BrandModelFactory(
        IBrandService brandService,
        IUrlRecordService urlRecordService,
        IPictureService pictureService,
        IStaticCacheManager staticCacheManager,
        MediaSettings mediaSettings)
    {
        _brandService = brandService;
        _urlRecordService = urlRecordService;
        _pictureService = pictureService;
        _staticCacheManager = staticCacheManager;
        _mediaSettings = mediaSettings;
    }

    public async Task<BrandDetailsModel> PrepareBrandDetailsModelAsync(Brand brand)
    {
        var seNameTask = _urlRecordService.GetSeNameAsync(brand);
        var picTask = GetBrandLogoAsync(brand);

        await Task.WhenAll(seNameTask, picTask);

        return new BrandDetailsModel
        {
            Id = brand.Id,
            Description = brand.Description,
            MetaKeywords = brand.MetaKeywords,
            MetaDescription = brand.MetaDescription,
            MetaTitle = brand.MetaTitle,
            Name = brand.Name,
            Picture = picTask.Result,
            SeName = seNameTask.Result,
        };
    }

    public async Task<IList<BrandProductModel>> PrepareProductBrandProductModelsAsync(int productId)
    {
        var brands = await _brandService.GetProductBrandsByProductIdAsync(productId);
        if (brands == null || brands.Count <= 0)
            return Array.Empty<BrandProductModel>();

        var tasks = brands.DistinctBy(x => x.Name, StringComparer.OrdinalIgnoreCase).Select(async brand =>
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
            .ThenBy(f => f.Name)
            .ToList();

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