namespace KedemMarket.Fairs.Domain;

public class FairVendorProductMap : BaseEntity
{
    public int FairId { get; set; }
    public int VendorId { get; set; }
    public int ProductId { get; set; }
    public bool? Declined { get; set; }
    public bool? Approved { get; set; }
    public DateTime? ApprovedOnUtc { get; set; }
    public DateTime? DeclinedOnUtc { get; set; }
    public bool IsAutoApproved { get; set; }
    public int FairAdminDisplayOrder { get; set; }
    public int FairVendorDisplayOrder { get; set; }
}