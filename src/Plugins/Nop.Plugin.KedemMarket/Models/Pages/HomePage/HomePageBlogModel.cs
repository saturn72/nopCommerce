namespace KedemMarket.Models.Pages.HomePage;

public record HomePageBlogModel : BaseNopEntityModel
{
    public string Title { get; set; } = string.Empty;
    public string Short { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? MetaKeywords { get; init; }
    public string? MetaDescription { get; init; }
    public string? MetaTitle { get; init; }
}
