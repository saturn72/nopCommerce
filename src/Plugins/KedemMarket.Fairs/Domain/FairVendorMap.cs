namespace KedemMarket.Fairs.Domain;

public class FairVendorMap : BaseEntity
{
    public int FairId { get; set; }
    public int VendorId { get; set; }
    public int DisplayOrder { get; set; }
    public bool AutoApproveProducts { get; set; }
    public Fair Fair { get; set; }
    public Vendor Vendor { get; set; }
}
