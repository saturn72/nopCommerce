using KedemMarket.Common.Models.Media;

namespace KedemMarket.Fairs.Models;
public record FairApiModel
{
    public virtual int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime? StartsOnUtc { get; set; }
    public DateTime? EndsOnUtc { get; set; }
    public GalleryItemModel? Image { get; set; }
    public string? Description { get; set; }
    public IEnumerable<string>? Tags { get; set; }
    public string Url { get; set; }
    public bool IsVirtual { get; set; }
    public bool IsFavorite { get; set; }
    public IEnumerable<FairVendorApiModel>? Vendors { get; set; }
}
