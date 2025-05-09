namespace KedemMarket.Fairs.Domain;

public class FairVendorProductMap : BaseEntity
{
    public int FairId { get; set; }
    public int VendorId { get; set; }
    public bool? Declined { get; set; }
    public bool Approved { get; set; }
    public bool PendingApproval { get; set; } = true;
    public DateTime? ApprovedOnUtc { get; set; }
    public DateTime? DeclinedOnUtc { get; set; }
    public bool IsAutoApproved { get; set; }
    public int DisplayOrder { get; set; }
    public int ProductId { get; set; }
    public decimal ProductPrice { get; set; }
    public string ProductName { get; set; }
}