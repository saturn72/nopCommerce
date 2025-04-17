using KedemMarket.Common.Models.Media;

namespace KedemMarket.Fairs.Models;

public record FairVendorApiModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public GalleryItemModel? Image { get; set; }
    public string? Description { get; set; }
    public IEnumerable<string>? Tags { get; set; }
}
