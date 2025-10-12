namespace KedemMarket.Models.Pages;
public record HomePageModel
{
    public IList<NavbarElementModel> Categories { get; set; } = [];
    public IList<SlideModel> Slides { get; set; } = [];
    public IList<VendorApiModel> Vendors { get; set; } = [];
}
