using KedemMarket.Common.Models.Media;

namespace KedemMarket.Fairs.Models;

public record FairVendorProductApiModel : BaseNopEntityModel
{
    public string Description { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string PriceText { get; set; }
    public int ProductId { get; set; }
    public GalleryItemModel Image { get; set; }
}
