
namespace KedemMarket.Models.Catalog;

public record CategoryApiModel : BaseNopEntityModel
{
    public List<CategorySlimApiModel> Breadcrumbs { get; init; }
    public string? Description { get; init; }
    public IEnumerable<ProductSlimApiModel> FeaturedProducts { get; init; }
    public string? ImageUrl { get; init; }
    public string? JsonLd { get; init; }
    public string? MetaKeywords { get; init; }
    public string? MetaDescription { get; init; }
    public string? MetaTitle { get; init; }
    public string? Name { get; init; }
    public IEnumerable<ProductInfoApiModel> Products { get; init; }
    public string? ThumbnailUrl { get; init; }
    public string? Slug { get; init; }
    public IEnumerable<CategorySlimApiModel> SubCategories { get; init; }
}
