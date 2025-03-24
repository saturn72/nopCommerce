namespace KedemMarket.Fair.Admin.Models.Vendors;

public record FairVendorModel : BaseNopEntityModel
{
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
}
