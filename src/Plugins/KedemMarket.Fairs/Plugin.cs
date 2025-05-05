using Nop.Core.Domain.Localization;
using Nop.Services.Customers;

namespace KedemMarket.Fairs;

public class Plugin : BasePlugin
{
    private readonly ILanguageService _languageService;
    private readonly IEnumerable<LocaleStringResource> _localeStringResources;
    private readonly ILocalizationService _localizationService;
    private readonly ICustomerService _customerService;
    private readonly CustomerRole _fairManagerCustomerRole = new()
    {
        Active = true,
        Name = FairConsts.CustomerRoleNames.FairManager,
        SystemName = FairConsts.CustomerRoleNames.FairManager,
    };
    public Plugin(
        ILanguageService languageService,
        ILocalizationService localizationService,
        ICustomerService customerService)
    {
        _languageService = languageService;
        _localeStringResources = GetLocaleStringResources();
        _localizationService = localizationService;
        _customerService = customerService;
    }

    private IEnumerable<LocaleStringResource> GetLocaleStringResources()
    {
        var languageIds = _languageService.GetAllLanguages(showHidden: true)
          .OrderBy(l => l.DisplayOrder).Select(x => x.Id);

        var res = new List<LocaleStringResource>();
        foreach (var lId in languageIds)
            res.AddRange(GetLocaleResourcesInfo(lId));

        return res;
    }

    private IEnumerable<LocaleStringResource> GetLocaleResourcesInfo(int languageId)
    {
        return new[]
           {
        new LocaleStringResource
        {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.list",
                ResourceValue = "Fairs list"
            },
        new LocaleStringResource {
            LanguageId = languageId,
            ResourceName =  "Admin.Fairs.Info",
            ResourceValue ="Fair Information",
        },
        new LocaleStringResource {
            LanguageId = languageId,
            ResourceName =  "Admin.Fairs.Location",
            ResourceValue ="Location",
        },
        new LocaleStringResource {
            LanguageId = languageId,
            ResourceName =  "Admin.Fairs.Vendors",
            ResourceValue ="Vendors",
        },
        new LocaleStringResource
        {
                LanguageId = languageId,
                ResourceName = "admin.fairs.list.searchfairname",
                ResourceValue = "Name"
            },
        new LocaleStringResource
        {
                LanguageId = languageId,
                ResourceName = "admin.fairs.List.SearchDateFilter",
                ResourceValue = "Expired"
            },
        new LocaleStringResource
        {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.List.SearchPublishedFilter",
                ResourceValue = "Published"
            },
        new LocaleStringResource
        {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.List.SearchDeletedFilter",
                ResourceValue = "Deleted"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.StartsOnUtc",
                ResourceValue = "Starts On Utc"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.EndsOnUtc",
                ResourceValue = "Ends On Utc"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.EndsOnUtc.MinimumLength",
                ResourceValue = "Ends On Utc field does not match the minimum length requirements of {0} hours"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Name",
                ResourceValue = "Name"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Name.Required",
                ResourceValue = "Name required"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Name.Unique",
                ResourceValue = "Fair with same name already exist. Fair names must be unique"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Published.EndsOnUtcRequired",
                ResourceValue = "Publish event requires valid Ends On Utc value"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Vendors",
                ResourceValue = "Vendors"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.DisplayOrder",
                ResourceValue = "Display Order"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Published",
                ResourceValue = "Published"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Description",
                ResourceValue = "Description"
            },

            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.PageSize",
                ResourceValue = "PageSize"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.AllowCustomersToSelectPageSize",
                ResourceValue = "Allow Customers To Select Page Size"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.PageSizeOptions",
                ResourceValue = "Page Size Options"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Vendors",
                ResourceValue = "Vendors"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Vendors.None",
                ResourceValue = "-"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Deleted",
                ResourceValue = "Deleted"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.LimitedToStores",
                ResourceValue = "Limited To Stores"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.AddNew",
                ResourceValue = "Add new Fair"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Fields.Id.Invalid",
                ResourceValue = "Invalid Id Value"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Added",
                ResourceValue = "Fair was added"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Failed",
                ResourceValue = "Failed to add fair"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Deleted",
                ResourceValue = "Fair deleted"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.EditFairDetails",
                ResourceValue = "Edit Fair Details"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Vendors.Vendor.Added",
                ResourceValue = "Vendor was added to the Fair"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Vendors.Vendor.AllVendorsAdded",
                ResourceValue = "All vendors were added to this Fair"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Vendors.Vendor.Delete",
                ResourceValue = "Vendor was deleted from fair"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.Vendors.Vendor.Updated",
                ResourceValue = "Fair's Vendor Updated successfuly"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.BackToList",
                ResourceValue = "Back to Fairs List"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.BackToFair",
                ResourceValue = "Back to Fair"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.FairVendor.Fields.Name",
                ResourceValue = "Name"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.FairVendor.Fields.DisplayOrder",
                ResourceValue = "Display Order"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.FairVendor.Fields.AutoApproveProducts",
                ResourceValue = "Auto Approve Products"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "admin.addproducttofairvendor.fields.searchproductname",
                ResourceValue = "Product Name"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "admin.addproducttofairvendor.fields.searchproducttypeid",
                ResourceValue = "Product Type"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "admin.addproducttofairvendor.fields.searchcategory",
                ResourceValue = "Category"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "admin.addproducttofairvendor.fields.searchmanufacturerid",
                ResourceValue = "Manufacturer"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fairs.FairVendorProduct.Add",
                ResourceValue = "Add Product to Fair"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.FairVendorProduct.Fields.Name",
                ResourceValue = "Product Name"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.FairVendorProduct.Fields.Approved",
                ResourceValue = "Approved"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.FairVendorProduct.Fields.DisplayOrder",
                ResourceValue = "Display Order"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.FairVendorProduct.Fields.Price",
                ResourceValue = "Price"
            },
        };
    }

    public override async Task InstallAsync()
    {
        await base.InstallAsync();
        await InsertLocaleResourcesAsync();

        var fmr = await _customerService.GetCustomerRoleBySystemNameAsync(_fairManagerCustomerRole.SystemName);
        if (fmr == null)
            await _customerService.InsertCustomerRoleAsync(_fairManagerCustomerRole);
    }

    public override async Task UninstallAsync()
    {
        await _customerService.DeleteCustomerRoleAsync(_fairManagerCustomerRole);
        await UninsertLocaleResourcesAsync();
        await base.UninstallAsync();
    }

    private async Task UninsertLocaleResourcesAsync()
    {
        var rns = _localeStringResources.Select(l => l.ResourceName).ToList();
        await _localizationService.DeleteLocaleResourcesAsync(rns);
    }

    private async Task InsertLocaleResourcesAsync()
    {
        var tasks = new List<Task>();
        foreach (var lr in _localeStringResources)
            tasks.Add(_localizationService.InsertLocaleStringResourceAsync(lr));
        await Task.WhenAll(tasks);
    }
}
