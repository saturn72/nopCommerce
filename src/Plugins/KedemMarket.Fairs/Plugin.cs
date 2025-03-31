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
                ResourceName = "admin.fair.list",
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
                ResourceName = "Admin.Fair.Fields.Vendors",
                ResourceValue = "Vendors"
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
                ResourceName = "Admin.Fair.Fields.Id.Invalid",
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
                ResourceName = "Admin.Fairs.BackTofAIR",
                ResourceValue = "Back to Fair"
            },
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.AddNew",
            //    ResourceValue = "Add Fair Vendor"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Edit",
            //    ResourceValue = "Edit Fair Vendor"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Vendors.AddNew",
            //    ResourceValue = "Add Vendor to Fair Vendor"
            //},
           
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Vendors.Deleted",
            //    ResourceValue = "Vendor was removed from Fair Vendor"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Vendors.SaveBeforeEdit",
            //    ResourceValue = "Save Before Edit"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Added",
            //    ResourceValue = "Fair Vendor Added successfuly"
            //},
            
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Edit",
            //    ResourceValue = "Edit Fair Vendor"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Alt",
            //    ResourceValue = "Alt"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Icon",
            //    ResourceValue = "Icon"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Icon.Required",
            //    ResourceValue = "Icon is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.ActiveIcon",
            //    ResourceValue = "Active Icon"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.ActiveIcon.Required",
            //    ResourceValue = "Active Icon is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Type",
            //    ResourceValue = "Type"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Type.Invalid",
            //    ResourceValue = "Value is invalid"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Type.Required",
            //    ResourceValue = "Value is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Value.Required",
            //    ResourceValue = "Value is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.FairInfoId.Required",
            //    ResourceValue = "Fair Id is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Index",
            //    ResourceValue = "Index"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Caption",
            //    ResourceValue = "Caption"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Caption.Required",
            //    ResourceValue = "Caption is required"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Caption.Unique",
            //    ResourceValue = "Caption should be unique for fair"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Tags",
            //    ResourceValue = "Tags"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Type",
            //    ResourceValue = "Type"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Value",
            //    ResourceValue = "Value"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Fields.Vendors",
            //    ResourceValue = "Vendors"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Vendors.Fields.VendorName",
            //    ResourceValue = "Vendor Name"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Vendors.Fields.IsFeaturedVendor",
            //    ResourceValue = "Featured Vendor"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Vendors.Fields.DisplayOrder",
            //    ResourceValue = "Display Order"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Info",
            //    ResourceValue = "Info"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Vendors",
            //    ResourceValue = "Vendors"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Vendors.List.SearchVendorName",
            //    ResourceValue = "Vendor Name"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Vendors.List.SearchFairVendor",
            //    ResourceValue = "Fair Vendor"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.Fairs.Vendors.Vendors.List.SearchVendorId",
            //    ResourceValue = "Vendor Id"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName ="Admin.FairVendor.Vendors.Fields.Published",
            //    ResourceValue = "Published"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Vendors.Fields.PublishPhone",
            //    ResourceValue = "Published Phone"
            //},
            //new LocaleStringResource
            //{
            //    LanguageId = languageId,
            //    ResourceName = "Admin.FairVendor.Vendors.Fields.PublishWhatsapp",
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
