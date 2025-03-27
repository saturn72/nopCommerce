using Nop.Web.Framework.Mvc.ModelBinding;

namespace KedemMarket.Fairs.Admin.Models;
public record FairSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.Fairs.List.SearchFairName")]
    public string Name { get; set; }
    [NopResourceDisplayName("Admin.Fairs.List.SearchDateFilter")]
    public string? DateFilter { get; set; }
    [NopResourceDisplayName("Admin.Fairs.List.SearchPublishedFilter")]
    public bool? PublishedFilter { get; set; }
    [NopResourceDisplayName("Admin.Fairs.List.SearchDeletedFilter")]
    public bool? DeletedFilter { get;set; }
}