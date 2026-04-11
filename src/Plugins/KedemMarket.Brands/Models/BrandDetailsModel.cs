using Nop.Web.Framework.Models;
using Nop.Web.Models.Catalog;

namespace KedemMarket.Brands.Models;
public record BrandDetailsModel : BaseNopEntityModel
{
    public string Description { get; set; }
    public string JsonLd { get; set; }
    public string MetaKeywords { get; set; }
    public string MetaDescription { get; set; }
    public string MetaTitle { get; set; }
    public string Name { get; init; }
    public PictureModel Picture { get; init; }
    public IList<ProductOverviewModel> Products { get; set; }
    public string SeName { get; init; }
}
