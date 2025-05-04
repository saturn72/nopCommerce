using KedemMarket.Common.Models.Media;

namespace KedemMarket.Fairs.Models;

public record FairVendorProductApiModel : BaseNopEntityModel
{
    public string Name { get; internal set; }
    public decimal Price { get; internal set; }
    public string Description { get; internal set; }
    public GalleryItemModel Picture { get; internal set; }
}
