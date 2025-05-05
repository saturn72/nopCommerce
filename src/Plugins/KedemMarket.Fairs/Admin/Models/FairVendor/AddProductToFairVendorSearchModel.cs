using Nop.Web.Framework.Mvc.ModelBinding;

namespace KedemMarket.Fairs.Admin.Models.FairVendor;

public partial record FairVendorProductAddPopupListModel : BasePagedListModel<FairVendorProductModel>
{
}

public record AddProductToFairVendorSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.AddProductToFairVendor.Fields.SearchProductName")]
    public string SearchProductName { get; set; }
    [NopResourceDisplayName("Admin.AddProductToFairVendor.Fields.SearchProductTypeId")]
    public int SearchProductTypeId { get; set; }
    public IList<SelectListItem> AvailableProductTypes { get; set; }

    [NopResourceDisplayName("Admin.AddProductToFairVendor.Fields.SearchCategory")]
    public int SearchCategoryId { get; set; }
    public IList<SelectListItem> AvailableCategories { get; set; }

    [NopResourceDisplayName("Admin.AddProductToFairVendor.Fields.SearchManufacturerId")]
    public int SearchManufacturerId { get; set; }
    public IList<SelectListItem> AvailableManufacturers { get; set; }
    public int FairId { get; set; }
    public int VendorId { get; set; }
}
