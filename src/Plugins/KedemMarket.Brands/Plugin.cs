using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace KedemMarket.Brands;

public class Plugin : BasePlugin
{
    private readonly ILocalizationService _localizationService;
    private readonly ISettingService _settingsService;

    public Plugin(
        ILocalizationService localizationService,
        ISettingService settingsService)
    {
        _localizationService = localizationService;
        _settingsService = settingsService;
    }

    public override async Task InstallAsync()
    {
        await _settingsService.SaveSettingAsync(new BrandsSettings());
        await AddLocaleResourcesAsync();
    }

    private async Task AddLocaleResourcesAsync()
    {
        Dictionary<string, string> resources = new()
        {
            {"Admin.Brands.NoBrands", "No Brands found."},
            {"Admin.Catalog.Brands", "Brands"},
            {"Admin.Catalog.Brands.BulkEdit", "Bulk Edit" },
            { "Admin.Catalog.Brands.List.ImportFromJsonTip", "Placeholder for a comment related to importing json file"},
            {"Admin.Catalog.Brands.SearchBrandName", "Brand Name" },
            {"Admin.Catalog.Brands.Fields.Comment", "Comment" },
            {"Admin.Catalog.Brands.Fields.DisplayOrder", "Display Order" },
            {"Admin.Catalog.Brands.Fields.Logo", "Logo" },
            {"Admin.Catalog.Brands.Fields.Name", "Name" },
            {"Admin.Catalog.Brands.Fields.SeName", "SeName" },
        };

        await _localizationService.AddOrUpdateLocaleResourceAsync(resources);
        await _settingsService.DeleteSettingAsync<BrandsSettings>();
    }
}