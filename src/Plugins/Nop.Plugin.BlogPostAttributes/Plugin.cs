using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Services.Localization;
using Nop.Services.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Saturn72.BlogPostExtensions.Components;

namespace Saturn72.BlogPostExtensions;

public class Plugin : BasePlugin, IWidgetPlugin
{
    private readonly ILocalizationService _localizationService;
    private readonly ISettingService _settingService;
    private readonly Dictionary<string, string> _locales;

    public Plugin(ILocalizationService localizationService, ISettingService settingService)
    {
        _localizationService = localizationService;
        _settingService = settingService;
        _locales = new Dictionary<string, string>
        {
            ["Plugins.BlogPostAttributes.Fields.Subtitle"] = "Subtitle",
            ["Plugins.BlogPostAttributes.Fields.Subtitle.Hint"] = "Enter a subtitle for the blog post. This will appear below the main title.",
            ["Plugins.BlogPostAttributes.Fields.MinSubtitleLength"] = "Min Subtitle Length",
            ["Plugins.BlogPostAttributes.Fields.MinSubtitleLength.Hint"] = "Enter the minimum allowed length for the subtitle.",
            ["Plugins.BlogPostAttributes.Fields.Picture"] = "Image",
            ["Plugins.BlogPostAttributes.Fields.Picture.Hint"] = "Upload or select an image to display with the blog post.",
            ["Plugins.BlogPostAttributes.Fields.Authors"] = "Authors",
            ["Plugins.BlogPostAttributes.Fields.Authors.Hint"] = "Select one or more authors for this blog post. Only users with permission to create blog posts are listed.",
            ["Plugins.BlogPostAttributes.Fields.Authors.NoAuthorsAvailable"] = "No authors available"
        };
    }
    public bool HideInWidgetList => false;

    public override string GetConfigurationPageUrl()
    {
        return "/Admin/BlogPostAttributesConfigure/Configure";
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            AdminWidgetZones.BlogPostDetailsBlock
        });
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(BlogPostExtensionsAdminWidgetViewComponent);
    }

    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(_locales);
        // Create and save default configuration
        var config = new BlogPostExtensionsConfiguration { MaxSubtitleLength = 256, MinSubtitleLength = 1 };
        await _settingService.SaveSettingAsync(config);
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _localizationService.DeleteLocaleResourcesAsync(_locales.Keys.ToList());
        // Remove configuration settings
        await _settingService.DeleteSettingAsync<BlogPostExtensionsConfiguration>();
        await base.UninstallAsync();
    }
}
