
namespace KedemMarket.Models.Vendors;
public record VendorOrderModel : BaseNopEntityModel
{
    public DateTime CreatedOn { get; set; }
    public IEnumerable<VendorOrderItemModel> Items { get; set; } = [];
    public string OrderStatus { get; set; }
    public string OrderTotal { get; set; }
}
