namespace KedemMarket.Controllers;

[Route("api/navbar")]
public class NavbarApiController : KmApiControllerBase
{
    private readonly KedemMarket.Factories.Navbar.INavbarFactory _navbarFactory;
    private readonly IWorkContext _workContext;

    public NavbarApiController(
        KedemMarket.Factories.Navbar.INavbarFactory navbarFactory, 
        IWorkContext workContext)
    {
        _navbarFactory = navbarFactory;
        _workContext = workContext;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GeNavbarInfoByNameAsync(string name)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == default)
            return BadRequest();

        if (name.HasNoValue())
            return BadRequest();

        var data = await _navbarFactory.PrepareNavbarApiModelByNameAsync(name);
        if (data == null)
            return BadRequest();
        return ToJsonResult(data);
    }
}
