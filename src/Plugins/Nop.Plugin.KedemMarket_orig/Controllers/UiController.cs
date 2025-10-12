namespace KedemMarket.Controllers;

[Route("ui")]
public class UiController : KmApiControllerBase
{
    private readonly IHomePageFactory _homePageFactory;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IEntityToModelFactory _entityToModelFactory;

    public UiController(
        IHomePageFactory homePageFactory,
        IWorkContext workContext,
        IUrlRecordService urlRecordService,
        IEntityToModelFactory entityToModelFactory)
    {
        _homePageFactory = homePageFactory;
        _workContext = workContext;
        _urlRecordService = urlRecordService;
        _entityToModelFactory = entityToModelFactory;
    }

    [HttpGet("homepage")]
    public async Task<IActionResult> GetHomePageAsync()
    {
        var model = await _homePageFactory.GetHomePageAsync();

        return ToJsonResult(model);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetProductBySlugAsync(string slug)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == default)
            return BadRequest();

        var ur = await _urlRecordService.GetBySlugAsync(slug);
        if (ur == null || ur.EntityId <= 0)
            return NotFound();

        var data = await _entityToModelFactory.GetModelEntityByTypeNameAndEntityId(ur.EntityName, ur.EntityId);

        if (data == default)
            return NotFound();

        var d = new
        {
            type = ur.EntityName,
            value = data,
        };
        return ToJsonResult(d);
    }
}