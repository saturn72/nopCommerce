using Nop.Web.Framework.Models;

namespace KedemMarket.Cms.Models.Media;

public record ImageModel: BaseNopEntityModel
{
    public required string ImageUrl { get; init; }
    public required string ThumbImageUrl { get; init; }
    public required string FullSizeImageUrl { get; init; }
    public required string Title { get; init; }
    public required string AlternateText { get; init; }
}


