using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Models.Blogs;
using Nop.Web.Framework.Components;
using Saturn72.BlogPostExtensions.Domain;
using Saturn72.BlogPostExtensions.Models;
using Saturn72.BlogPostExtensions.Services;
using Saturn72.BlogPostExtensions.Views;

namespace Saturn72.BlogPostExtensions.Components;

public class BlogPostExtensionsAdminWidgetViewComponent : NopViewComponent
{
    private readonly ICustomerService _customerService;
    private readonly IPermissionService _permissionService;
    private readonly IWorkContext _workContext;
    private readonly IBlogPostAttributeService _blogPostAttributeService;
    private readonly ISettingService _settingService;

    public BlogPostExtensionsAdminWidgetViewComponent(
        ICustomerService customerService,
        IPermissionService permissionService,
        IWorkContext workContext,
        IBlogPostAttributeService blogPostAttributeService,
        ISettingService settingService)
    {
        _customerService = customerService;
        _permissionService = permissionService;
        _workContext = workContext;
        _blogPostAttributeService = blogPostAttributeService;
        _settingService = settingService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var bp = additionalData as BlogPostModel;
        var isNew = bp!.Id == 0;

        var bpa = isNew ?
            new BlogPostAttributes
            {
                AuthorIds = (await _workContext.GetCurrentCustomerAsync()).Id.ToString()
            } :
            await _blogPostAttributeService.GetByBlogPostIdAsync(bp.Id) ?? new BlogPostAttributes();

        var model = new BlogPostAttributesModel
        {
            BlogPostId = bpa.BlogPostId,
            Subtitle = bpa.Subtitle,
            PictureId = bpa.PictureId,
        };


        // Load users with permission to create blog posts
        var allCustomers = await _customerService.GetAllCustomersAsync();
        var availableAuthors = new List<SelectListItem>();
        foreach (var customer in allCustomers)
        {
            if (await _permissionService.AuthorizeAsync(StandardPermission.ContentManagement.BLOG_CREATE_EDIT_DELETE, customer))
            {
                availableAuthors.Add(new SelectListItem
                {
                    Text = GetAuthorName(customer),
                    Value = customer.Id.ToString()
                });
            }
        }
        model.AvailableAuthors = availableAuthors;

        if (!string.IsNullOrEmpty(bpa.AuthorIds) && bpa.AuthorIds.Length > 0)
        {
            var aIds = bpa.AuthorIds.Split(',').Select(c => c.Trim()).ToList();
            model.SelectedAuthorIds = model.AvailableAuthors.Where(c => aIds.Contains(c.Value)).ToList();
        }

        var settings = await _settingService.LoadSettingAsync<BlogPostExtensionsConfiguration>();
        model.MinSubtitleLength = settings.MinSubtitleLength;
        model.MaxSubtitleLength = settings.MaxSubtitleLength;

        return View(ViewUtil.GetViewPath("_AdminWidget"), model);
    }

    private string GetAuthorName(Customer customer)
    {
        var e = customer.Email.Trim();
        var n = customer.Username.Trim();
        return e == n? e: $"{e}/{n}";
    }
}
