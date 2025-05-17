namespace KedemMarket.Fairs.Admin.Models.FairVendor;
public record FairVendorProductDeclineModel : BaseNopModel
{
    public string? DeclineReason { get; set; }
}
