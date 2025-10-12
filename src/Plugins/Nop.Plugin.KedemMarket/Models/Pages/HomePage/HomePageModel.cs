namespace KedemMarket.Models.Pages.HomePage;
public record HomePageModel
{
    public IEnumerable<HomePageBlogModel> Blogs { get; set; } = [];
    public IList<NavbarElementModel> Categories { get; set; } = [];
    public IList<SlideModel> Slides { get; set; } = [];
    public IList<VendorApiModel> Vendors { get; set; } = [];
}
