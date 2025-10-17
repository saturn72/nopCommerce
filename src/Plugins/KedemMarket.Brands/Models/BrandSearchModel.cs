using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace KedemMarket.Brands.Models;
public record class BrandSearchModel : BaseSearchModel
{

    [NopResourceDisplayName("Admin.Catalog.Brands.SearchBrandName")]
    public string[]? SearchBrandNames { get; set; }

    public bool IsLoggedInAsVendor { get; set; }
    public bool AllowVendorsToImportBrands { get; set; }
}