namespace KedemMarket.Models.Navbar;

public record NavbarVendorModel
{
    public required int Id { get; init; }
    public string? Phone { get; init; }
    public GalleryItemModel? Picture { get; init; }
    public IEnumerable<ProductSlimApiModel>? Products { get; init; }
    public string? Name { get; init; }
    public string? ShortDescription { get; init; }
    public string? Whatsapp { get; init; }
}