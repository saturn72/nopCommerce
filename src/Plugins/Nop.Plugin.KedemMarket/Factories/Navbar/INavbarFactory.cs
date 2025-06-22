namespace KedemMarket.Factories.Navbar;
public interface INavbarFactory
{
    Task<NavbarModel> PrepareNavbarModelByNameAsync(string name);
}
