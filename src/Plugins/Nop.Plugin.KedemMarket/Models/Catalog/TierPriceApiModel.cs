namespace KedemMarket.Models.Catalog;

public record TierPriceApiModel
{
    public required decimal Price { get; set; }
    public required string PriceText { get; set; }
    public required int Quantity { get; set; }
}