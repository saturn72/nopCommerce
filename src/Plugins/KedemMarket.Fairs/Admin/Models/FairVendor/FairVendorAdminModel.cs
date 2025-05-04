using System.ComponentModel;

namespace KedemMarket.Fairs.Admin.Models.FairVendor;

public record FairVendorAdminModel : BaseNopEntityModel
{
    [DisplayName("Admin.FairVendor.Fields.Name")]
    public string Name { get; set; }
    [DisplayName("Admin.FairVendor.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }
    [DisplayName("Admin.FairVendor.Fields.AutoApproveProducts")]
    public bool AutoApproveProducts { get; set; }
}
