using KedemMarket.Cms.Factories.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KedemMarket.Cms.Controllers;

[Route("api/cms")]
public class CmsController : ControllerBase
{
    private readonly ICmsPagesFactory _cmsPagesFactory;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;
    private readonly ICmsPageModelFactory _cmsPageModelFactory;
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

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
    
    protected internal static JsonResult ToJsonResult(object body)
    {
        return new(body, _jsonSerializerSettings);
    }
}