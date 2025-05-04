namespace KedemMarket.Fairs.Admin.Models.FairVendor;

public partial record AddProductToFairVendorModel : BaseNopModel
{
    public AddProductToFairVendorModel()
    {
        SelectedProductIds = new List<int>();
    }
    public int FairId { get; set; }
    public int VendorId { get; set; }
    public IList<int> SelectedProductIds { get; set; }
}