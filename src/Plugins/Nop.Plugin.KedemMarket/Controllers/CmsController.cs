namespace KedemMarket.Controllers;

[Route("cms")]
public class CmsController : KmApiControllerBase
{
    private readonly ICmsPagesFactory _cmsPagesFactory;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;
    private readonly ICmsPageModelFactory _cmsPageModelFactory;

    public CmsController(
        ICmsPagesFactory cmsPagesFactory,
        IWorkContext workContext,
        IUrlRecordService urlRecordService,
        ICmsPageModelFactory entityToModelFactory)
    {
        _cmsPagesFactory = cmsPagesFactory;
        _workContext = workContext;
        _urlRecordService = urlRecordService;
        _cmsPageModelFactory = entityToModelFactory;
    }

    [HttpGet("homepage")]
    public async Task<IActionResult> GetHomePageAsync()
    {
        var model = await _cmsPagesFactory.GetHomePageAsync();

        return ToJsonResult(model);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetCmsPageBySlugAsync(string slug)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == default)
            return BadRequest();

        var ur = await _urlRecordService.GetBySlugAsync(slug);
        if (ur == null || ur.EntityId <= 0)
            return NotFound();

        var data = await _cmsPageModelFactory.GetCmsPageModelEntityByTypeNameAndEntityIdAsync(ur.EntityName, ur.EntityId);

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