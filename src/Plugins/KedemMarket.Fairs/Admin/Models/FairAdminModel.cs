using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace KedemMarket.Fairs.Admin.Models;
public record FairAdminModel : BaseNopEntityModel
{
    public string Name { get; set; }
    public bool Deleted { get; set; }
    public string Description { get; set; }
    public bool IsVirtual { get; set; }
    public bool Published { get; set; }
    [UIHint("Picture")]
    [NopResourceDisplayName("KedemMarket.Fairs.Admin.Models.FairAdminModel.Fields.Picture")]
    public int PictureId { get; set; }
    public DateTime? StartsOnUtc { get; set; }
    public DateTime? EndsOnUtc { get; set; }
    public AddressModel Address { get; set; }
    public string Url { get; set; }
    public IList<int> AdminIds { get; set; }
    public IList<VendorModel> Vendors { get; set; } = [];
    public FairVendorSearchModel FairVendorSearchModel { get; set; } = new();
}
