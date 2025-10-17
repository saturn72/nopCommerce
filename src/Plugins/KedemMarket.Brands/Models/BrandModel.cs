using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace KedemMarket.Brands.Models;
public record BrandModel : BaseNopEntityModel
{
    [NopResourceDisplayName("Admin.Catalog.Brands.Fields.Comment")]
    public string Comment { get; init; }
    [NopResourceDisplayName("Admin.Catalog.Brands.Fields.DisplayOrder")]
    public int DisplayOrder { get; init; }
    [NopResourceDisplayName("Admin.Catalog.Brands.Fields.Logo")]
    public string LogoUrl { get; init; }
    [NopResourceDisplayName("Admin.Catalog.Brands.Fields.Name")]
    public string Name { get; init; }
    [NopResourceDisplayName("Admin.Catalog.Brands.Fields.SeName")]
    public string SeName { get; init; }
}
