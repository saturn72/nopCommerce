using Microsoft.AspNetCore.StaticFiles;
using SkiaSharp;

namespace KedemMarket.Brands.Services;

public class BrandImportExportManager : IBrandImportExportManager
{
    private static SemaphoreSlim _semaphore = new(50); // Max 50 concurrent
    private readonly IBrandService _brandService;
    private readonly IRepository<Brand> _brandRepository;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly BrandsSettings _brandsSettings;
    private readonly IPictureService _pictureService;

    public BrandImportExportManager(
        IBrandService brandService,
        IRepository<Brand> brandRepository,
        IUrlRecordService urlRecordService,
        IHttpClientFactory httpClientFactory,
        BrandsSettings brandsSettings,
        IPictureService pictureService)
    {
        _brandService = brandService;
        _brandRepository = brandRepository;
        _urlRecordService = urlRecordService;
        _httpClientFactory = httpClientFactory;
        _brandsSettings = brandsSettings;
        _pictureService = pictureService;
    }

    public async Task ImportFromJsonAsync(Stream stream)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
        };

        var brandImportModels = JsonSerializer.Deserialize<List<BrandImportModel>>(stream, options);

        var names = brandImportModels.Where(b => !string.IsNullOrEmpty(b.Name) && !string.IsNullOrWhiteSpace(b.Name)).Select(b => b.Name.Trim()).ToArray();

        var toUpdate = new List<BrandImportModel>();
        var toCreate = new List<BrandImportModel>(brandImportModels);

        var dbBrands = await _brandService.GetAllBrandsAsync(names: names);
        if (dbBrands.Any())
        {
            foreach (var bim in brandImportModels)
            {
                var dbBrand = dbBrands.FirstOrDefault(b => b.Name.Equals(bim.Name, StringComparison.OrdinalIgnoreCase));

                if (dbBrand != null)
                {
                    toCreate.Remove(bim);
                    bim.DbEntity = dbBrand;
                    toUpdate.Add(bim);
                }
            }
        }

        var tasks = (await CreateBrandsAsync(toCreate)).ToList();
        tasks.AddRange(UpdateBrandsAsync(toUpdate));
        await Task.WhenAll(tasks);
    }

    private async Task<IList<Task>> CreateBrandsAsync(List<BrandImportModel> toCreate)
    {
        if (toCreate == null || toCreate.Count == 0)
            return [];

        var brands = toCreate.Select(bim => new Brand
        {
            Comment = bim.Comment,
            DisplayOrder = bim.DisplayOrder,
            Name = bim.Name,
        }).ToList();

        await _brandRepository.InsertAsync(brands);


        var tasks = new List<Task>();
        foreach (var cb in brands)
        {
            var urlTask = Task.Run(async () =>
            {
                var seName = await _urlRecordService.ValidateSeNameAsync(cb, string.Empty, cb.Name, true);
                await _urlRecordService.SaveSlugAsync(cb, seName, 0);
            });
            tasks.Add(urlTask);

            var imageTask = Task.Run(async () =>
            {
                var bim = toCreate.FirstOrDefault(c => c.Name.Equals(cb.Name, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(bim.Image) || string.IsNullOrWhiteSpace(bim.Image))
                    return;

                var picBytes = await GetLogoBytesAsync(bim.Image);
                if (picBytes != null && picBytes.Length != 0)
                    await InsertBrandLogoAsync(picBytes, cb);
            });
            tasks.Add(imageTask);
        }
        return tasks;
    }

    protected async Task InsertBrandLogoAsync(byte[] pictureBytes, Brand brand)
    {
        var p = await _pictureService.InsertPictureAsync(
            pictureBytes, "image/webp",
            $"{brand.Name}-logo",
            brand.Name, brand.Name,
            validateBinary: false);
        brand.LogoImageId = p.Id;
        await _brandRepository.UpdateAsync(brand);
    }

    private IList<Task> UpdateBrandsAsync(List<BrandImportModel> toUpdate)
    {
        var tasks = new List<Task>();
        if (toUpdate == null || toUpdate.Count == 0)
            return tasks;

        var brands = new List<Brand>();
        foreach (var bim in toUpdate)
        {
            var brand = bim.DbEntity;
            brand.Comment = bim.Comment;
            brand.DisplayOrder = bim.DisplayOrder;
            brand.UpdatedOnUtc = DateTime.UtcNow;
            brands.Add(brand);

            var picTask = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(bim.Image) || string.IsNullOrWhiteSpace(bim.Image))
                    return;

                var picBytes = await GetLogoBytesAsync(bim.Image);
                if (picBytes == null || picBytes.Length == 0)
                    return;

                var curPicture = await _pictureService.GetPictureBinaryByPictureIdAsync(brand.LogoImageId);
                var t = curPicture == null ?
                    InsertBrandLogoAsync(picBytes, brand) :
                        _pictureService.UpdatePictureAsync(
                            brand.LogoImageId,
                            picBytes, "image/webp",
                            seoFilename: $"{brand.Name}-logo",
                            altAttribute: brand.Name,
                            titleAttribute: brand.Name,
                            validateBinary: false);
                await t;
            });
            tasks.Add(picTask);

            var urlTask = Task.Run(async () =>
            {
                var seName = await _urlRecordService.GetSeNameAsync(brand);
                if (string.IsNullOrEmpty(seName) || string.IsNullOrWhiteSpace(seName))
                    await _urlRecordService.SaveSlugAsync(brand, seName, 0);
            });
            tasks.Add(urlTask);

        }

        tasks.Add(_brandRepository.UpdateAsync(brands));
        return tasks;
    }
    protected virtual string GetMimeTypeFromFileName(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }

    protected async Task<byte[]?> GetImageBytesAsync(string imageUrl)
    {
        await _semaphore.WaitAsync(); // Waits if limit reached
        try
        {
            using var httpClient = _httpClientFactory.CreateClient(name: KedemMarket.Brands.Infrastructure.NopStartup.HTTP_CLIENT_NAME);
            using var response = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsByteArrayAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    protected virtual async Task<byte[]> GetLogoBytesAsync(string imageUrl)
    {
        var imgBytes = await GetImageBytesAsync(imageUrl);
        if (imgBytes?.Length == 0)
            return null;

        var mimeType = GetMimeTypeFromFileName(imageUrl);
        var picBytes = await _pictureService.ValidatePictureAsync(imgBytes, mimeType, imageUrl);
        if (picBytes == null || !picBytes.Any())
            return null;

        using var imgStream = new MemoryStream(picBytes);
        using var codec = SKCodec.Create(imgStream);
        if (codec == null)
            return null;

        using var originalBitmap = SKBitmap.Decode(codec);
        if (originalBitmap == null)
            return null;

        var scale = Math.Min(
            (float)_brandsSettings.MaxLogoWidth / originalBitmap.Width,
            (float)_brandsSettings.MaxLogoHeight / originalBitmap.Height);

        var resizedWidth = (int)Math.Round(originalBitmap.Width * scale);
        var resizedHeight = (int)Math.Round(originalBitmap.Height * scale);

        if (scale >= 1.0f)
        {
            resizedWidth = originalBitmap.Width;
            resizedHeight = originalBitmap.Height;
        }

        var info = new SKImageInfo(resizedWidth, resizedHeight);
        using var resizedBitmap = new SKBitmap(info);

        using (var canvas = new SKCanvas(resizedBitmap))
        {
            var srcRect = new SKRect(0, 0, originalBitmap.Width, originalBitmap.Height);
            var destRect = new SKRect(0, 0, resizedWidth, resizedHeight);
            canvas.DrawBitmap(originalBitmap, srcRect, destRect);
        }

        using var image = SKImage.FromBitmap(resizedBitmap);
        using var encodedData = image.Encode(SKEncodedImageFormat.Webp, quality: 100);

        if (encodedData == null)
            return null;

        var outputStream = new MemoryStream();
        encodedData.SaveTo(outputStream);
        outputStream.Position = 0;

        return outputStream.ToArray();
    }

    internal class BrandImportModel
    {
        public string Comment { get; set; }
        public int DisplayOrder { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public Brand DbEntity { get; set; }
    }
}
