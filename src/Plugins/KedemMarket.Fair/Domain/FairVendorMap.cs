namespace KedemMarket.Fair.Domain;

public class FairVendorMap : BaseEntity
{
    public int FairInfoId { get; set; }
    public int VendorId { get; set; }
    public int DisplayIndex { get; set; }
}
