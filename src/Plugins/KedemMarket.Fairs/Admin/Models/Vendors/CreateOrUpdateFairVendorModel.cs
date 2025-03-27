namespace KedemMarket.Fairs.Admin.Models.Vendors;

public record CreateOrUpdateFairVendorModel : BaseNopEntityModel
{
    public int FairId { get; set; }
    public int VendorId { get; set; }
    public string VendorName { get; set; }
    public int DisplayOrder { get; set; }
    public IList<SelectListItem> AvailableVendors { get; set; }
}
