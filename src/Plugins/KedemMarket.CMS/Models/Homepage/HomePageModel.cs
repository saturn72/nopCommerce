using Nop.Web.Models.Catalog;

namespace KedemMarket.Cms.Models.Pages.HomePage;
public record HomePageModel
{
    public IList<ProductOverviewModel> Products { get; set; } = [];
    public IList<CategoryModel> Categories { get; set; } = [];
}
