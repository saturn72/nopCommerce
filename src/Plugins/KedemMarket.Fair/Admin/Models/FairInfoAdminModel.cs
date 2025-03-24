namespace KedemMarket.Fair.Admin.Models;
public record FairInfoAdminModel : BaseNopEntityModel
{
    public string Name { get; set; }
    public bool Deleted { get; set; }
    public string Description { get; set; }
    public bool Published { get; set; }
    public DateTime? StartsOnUtc { get; set; }
    public DateTime? EndsOnUtc { get; set; }
    public Address Address { get; set; }
    public IList<int> AdminIds { get; set; }
    public IList<VendorModel> Vendors { get; set; } = [];
    public FairVendorSearchModel FairVendorSearchModel { get; set; } = new();
}
