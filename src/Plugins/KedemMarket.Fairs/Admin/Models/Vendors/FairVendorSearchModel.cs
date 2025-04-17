using Nop.Web.Framework.Mvc.ModelBinding;

namespace KedemMarket.Fairs.Admin.Models.Vendors;

public record  ProductVendorListModel:BasePagedListModel<FairVendorAdminModel>
{
}

public record FairVendorSearchModel : BaseSearchModel
{
    public int FairId { get; set; }
    //[NopResourceDisplayName("Admin.Fair.Fields.PageSize")]
    [NopResourceDisplayName("Admin.Fair.Fields.AllowCustomersToSelectPageSize")]
    public bool AllowCustomersToSelectPageSize { get; set; }

    //[NopResourceDisplayName("Admin.Fair.Fields.PageSizeOptions")]
    //public string PageSizeOptions { get; set; }
    [NopResourceDisplayName("Admin.Fair.Fields.Vendors")]
    public IList<FairVendorAdminModel> Vendors { get; set; } = [];
}
