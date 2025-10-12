using Microsoft.AspNetCore.Mvc;
using Nop.Services.Configuration;
using Nop.Web.Framework.Mvc.Filters;
using Saturn72.BlogPostExtensions.Models;
using Saturn72.BlogPostExtensions.Views;
using Nop.Services.Security;

namespace Saturn72.BlogPostExtensions.Controllers;

[Area("Admin")]
public class BlogPostAttributesConfigureController : Controller
{

    private readonly ISettingService _settingService;

    public BlogPostAttributesConfigureController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet]
    [CheckPermission(StandardPermission.System.MANAGE_APP_SETTINGS)]
    public async Task<IActionResult> Configure()
    {
        var settings = await _settingService.LoadSettingAsync<BlogPostExtensionsConfiguration>();
        var model = new BlogPostExtensionsConfigurationModel
        {
            MaxSubtitleLength = settings.MaxSubtitleLength,
            MinSubtitleLength = settings.MinSubtitleLength
        };
        return View(ViewUtil.GetViewPath("Configure"), model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.System.MANAGE_APP_SETTINGS)]
    public async Task<IActionResult> Configure(BlogPostExtensionsConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var settings = await _settingService.LoadSettingAsync<BlogPostExtensionsConfiguration>();
        settings.MaxSubtitleLength = model.MaxSubtitleLength;
        settings.MinSubtitleLength = model.MinSubtitleLength;

        await _settingService.SaveSettingAsync(settings);
        return await Configure();
    }
}
