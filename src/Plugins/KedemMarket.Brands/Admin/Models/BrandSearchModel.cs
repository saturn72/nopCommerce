using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace KedemMarket.Brands.Admin.Models;
public record class BrandSearchModel : BaseSearchModel
{

    [NopResourceDisplayName("Admin.Catalog.Brands.SearchBrandName")]
    public string SearchBrandName { get; set; }

    public bool IsLoggedInAsVendor { get; set; }
    public bool AllowVendorsToImportBrands { get; set; }
}