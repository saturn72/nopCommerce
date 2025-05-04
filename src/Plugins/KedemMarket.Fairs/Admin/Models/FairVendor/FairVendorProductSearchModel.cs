namespace KedemMarket.Fairs.Admin.Models.FairVendor;

public record FairVendorProductSearchModel : BaseSearchModel
{
    public Fair? Fair { get; set; }
    public Vendor? Vendor { get; set; }
    public int FairId { get; set; }
    public int VendorId { get; set; }
    public bool? IsApprovedFilter { get; set; }
}
