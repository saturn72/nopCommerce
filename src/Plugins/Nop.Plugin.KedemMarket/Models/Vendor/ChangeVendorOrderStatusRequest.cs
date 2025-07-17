namespace KedemMarket.Models.Vendor;
public record ChangeVendorOrderStatusRequest
{
    public int OrderId { get; set; }
    public string Status { get; set; }
}
