using Nop.Web.Framework.Mvc.ModelBinding;

namespace KedemMarket.Fairs.Admin.Models.FairVendor;

public record ProductVendorListModel : BasePagedListModel<FairVendorAdminModel>
{
}

public record FairVendorSearchModel : BaseSearchModel
{
    public int FairId { get; set; }
    [NopResourceDisplayName("Admin.Fairs.Fields.AllowCustomersToSelectPageSize")]
    public bool AllowCustomersToSelectPageSize { get; set; }

    [NopResourceDisplayName("Admin.Fairs.Fields.Vendors")]
    public IList<FairVendorAdminModel> Vendors { get; set; } = [];
}
