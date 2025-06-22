namespace KedemMarket.Controllers;

[Route("ui/homepage")]
public class UiController : KmApiControllerBase
{
    private readonly IHomePageFactory _homePageFactory;

    public UiController(IHomePageFactory homePageFactory)
    {
        _homePageFactory = homePageFactory;
    }
    [HttpGet]
    public async Task<IActionResult> GetHomePageAsync()
    {
        var model = await _homePageFactory.GetHomePageAsync();

        return ToJsonResult(model);
    }
}