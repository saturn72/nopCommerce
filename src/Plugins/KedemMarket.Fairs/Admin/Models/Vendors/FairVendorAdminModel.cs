namespace KedemMarket.Fairs.Admin.Models.Vendors;

public record FairVendorAdminModel : BaseNopEntityModel
{
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
}
