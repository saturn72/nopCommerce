
namespace KedemMarket.Fairs.Admin.Models.FairVendor;

public record FairVendorProductModel : BaseNopEntityModel
{
    public int FairId { get; set; }
    public int VendorId { get; set; }
    public int ProductId { get; set; }
    public bool Approved { get; set; }
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public string FormattedPrice { get; set; }
}
