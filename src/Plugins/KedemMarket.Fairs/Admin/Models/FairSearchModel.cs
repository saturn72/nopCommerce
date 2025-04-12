using Nop.Web.Framework.Mvc.ModelBinding;

namespace KedemMarket.Fairs.Admin.Models;
public record FairSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.Fairs.List.SearchFairName")]
    public string Name { get; set; }
    [NopResourceDisplayName("Admin.Fairs.List.SearchStartsOnUtcFilter")]
    public DateTime? StartsOnUtc { get; set; }
    [NopResourceDisplayName("Admin.Fairs.List.SearchEndsOnUtcFilter")]
    public DateTime? EndsOnUtc { get; set; }
    [NopResourceDisplayName("Admin.Fairs.List.SearchPublishedFilter")]
    public bool? IsPublished { get; set; }
    [NopResourceDisplayName("Admin.Fairs.List.SearchDeletedFilter")]
    public bool? IsDeleted { get;set; }
}