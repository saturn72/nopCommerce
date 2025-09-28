namespace KedemMarket.Models.Catalog;

public record CategorySlimApiModel : BaseNopEntityModel
{
    public string? Name { get; init; }
    public string? Slug { get; init; }
    public string? Description { get; init; }
    public string? ThumbnailUrl { get; init; }
}
