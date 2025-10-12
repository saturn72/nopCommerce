

using KedemMarket.Models.Pages.HomePage;

namespace KedemMarket.Factories.Pages;
public interface ICmsPagesFactory
{
    public Task<HomePageModel> GetHomePageAsync();
}
