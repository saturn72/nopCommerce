namespace KedemMarket.Models.Vendors;

public record VendorOrderItemModel : BaseNopEntityModel
{
    public string? AttributeInfo { get; set; }
    public int Quantity { get; set; }
    public string? ItemOrderStatus { get; set; }
    public string? PictureThumbnailUrl { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
}
