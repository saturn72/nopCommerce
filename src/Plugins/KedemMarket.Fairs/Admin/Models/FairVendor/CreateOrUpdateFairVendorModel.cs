namespace KedemMarket.Fairs.Admin.Models.FairVendor;

public record CreateOrUpdateFairVendorModel : BaseNopEntityModel
{
    public bool AutoApproveProducts { get; set; }
    public int FairId { get; set; }
    public int VendorId { get; set; }
    public string VendorName { get; set; }
    public int DisplayOrder { get; set; }
    public IList<SelectListItem> AvailableVendors { get; set; }
    public FairVendorProductSearchModel FairVendorProductSearchModel { get; set; }
}
