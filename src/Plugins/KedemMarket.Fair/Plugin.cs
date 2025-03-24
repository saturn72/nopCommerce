using Nop.Core.Domain.Localization;
using Nop.Services.Customers;
using Nop.Services.Localization;

namespace KedemMarket.Fair;

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
                ResourceName = "admin.fair.list",
                ResourceValue = "Fairs list"
            },
        new LocaleStringResource {
            LanguageId = languageId,
            ResourceName =  "admin.fairs.info",
            ResourceValue ="Fair Info",
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
                ResourceName = "Admin.Fair.Fields.StartsOnUtc",
                ResourceValue = "Starts On Utc"
            },
             new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.EndsOnUtc",
                ResourceValue = "Ends On Utc"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.EndsOnUtc.MinimumLength",
                ResourceValue = "Ends On Utc field does not match the minimum length requirements of {0} hours"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Name",
                ResourceValue = "Name"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Name.Required",
                ResourceValue = "Name required"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Name.Unique",
                ResourceValue = "Fair with same name already exist. Fair names must be unique"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Published.EndsOnUtcRequired",
                ResourceValue = "Publish event requires valid Ends On Utc value"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Elements",
                ResourceValue = "Elements"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.DisplayOrder",
                ResourceValue = "Display Order"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Published",
                ResourceValue = "Published"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Description",
                ResourceValue = "Description"
            },

            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.PageSize",
                ResourceValue = "PageSize"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.AllowCustomersToSelectPageSize",
                ResourceValue = "Allow Customers To Select Page Size"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.PageSizeOptions",
                ResourceValue = "Page Size Options"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Vendors",
                ResourceValue = "Vendors"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Vendors.None",
                ResourceValue = "-"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.Deleted",
                ResourceValue = "Deleted"
            },
            new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = "Admin.Fair.Fields.LimitedToStores",
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
                ResourceName = "Admin.Fairs.Elements",
                ResourceValue = "Fair Elements"
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
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.AddNew",
            //    ResourceValue = "Add Fair Element"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Edit",
            //    ResourceValue = "Edit Fair Element"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Vendors.AddNew",
            //    ResourceValue = "Add Vendor to Fair Element"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Vendors.Added",
            //    ResourceValue = "Vendor was added to Fair Element"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Vendors.Deleted",
            //    ResourceValue = "Vendor was removed from Fair Element"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Vendors.SaveBeforeEdit",
            //    ResourceValue = "Save Before Edit"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Added",
            //    ResourceValue = "Fair Element Added successfuly"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Updated",
            //    ResourceValue = "Fair Element Updated successfuly"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Edit",
            //    ResourceValue = "Edit Fair Element"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Alt",
            //    ResourceValue = "Alt"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Icon",
            //    ResourceValue = "Icon"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Icon.Required",
            //    ResourceValue = "Icon is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.ActiveIcon",
            //    ResourceValue = "Active Icon"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.ActiveIcon.Required",
            //    ResourceValue = "Active Icon is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Type",
            //    ResourceValue = "Type"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Type.Invalid",
            //    ResourceValue = "Value is invalid"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Type.Required",
            //    ResourceValue = "Value is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Value.Required",
            //    ResourceValue = "Value is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.FairInfoId.Required",
            //    ResourceValue = "Fair Id is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Index",
            //    ResourceValue = "Index"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Caption",
            //    ResourceValue = "Caption"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Caption.Required",
            //    ResourceValue = "Caption is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Caption.Unique",
            //    ResourceValue = "Caption should be unique for fair"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Tags",
            //    ResourceValue = "Tags"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Type",
            //    ResourceValue = "Type"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Value",
            //    ResourceValue = "Value"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Fields.Vendors",
            //    ResourceValue = "Vendors"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Vendors.Fields.VendorName",
            //    ResourceValue = "Vendor Name"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Vendors.Fields.IsFeaturedVendor",
            //    ResourceValue = "Featured Vendor"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Vendors.Fields.DisplayOrder",
            //    ResourceValue = "Display Order"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Info",
            //    ResourceValue = "Info"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.BackToList",
            //    ResourceValue = "Back to Fairs List"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Vendors",
            //    ResourceValue = "Vendors"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Vendors.List.SearchVendorName",
            //    ResourceValue = "Vendor Name"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Vendors.List.SearchFairElement",
            //    ResourceValue = "Fair Element"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Elements.Vendors.List.SearchVendorId",
            //    ResourceValue = "Vendor Id"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName ="Admin.FairElement.Vendors.Fields.Published",
            //    ResourceValue = "Published"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Vendors.Fields.PublishPhone",
            //    ResourceValue = "Published Phone"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairElement.Vendors.Fields.PublishWhatsapp",
            //    ResourceValue = "Published Whatsapp"
            //},
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
