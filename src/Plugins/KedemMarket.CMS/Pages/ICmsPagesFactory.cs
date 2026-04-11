

using KedemMarket.Cms.Models.Pages.HomePage;

namespace KedemMarket.Cms.Factories.Pages;
public interface ICmsPagesFactory
{
    public Task<HomePageModel> GetHomePageAsync();
}
