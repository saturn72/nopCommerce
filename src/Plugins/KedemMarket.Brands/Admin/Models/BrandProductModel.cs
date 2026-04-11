using Nop.Web.Framework.Models;

namespace KedemMarket.Brands.Admin.Models;

public record BrandProductModel : BaseNopEntityModel
{
    public int DisplayOrder { get; init; }
    public PictureModel Logo{ get; init; }
    public string Name { get; init; }
    public string SeName { get; init; }
}