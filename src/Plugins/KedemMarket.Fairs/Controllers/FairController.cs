using KedemMarket.Fairs.Factories;
using KedemMarket.Fairs.Models;
using KM.Common.Controllers;

namespace KedemMarket.Fairs.Controllers;

[Route("api/fairs")]
public class FairController : KedemMarketApiControllerBase
{
    private readonly IFairService _fairService;
    private readonly IFairApiFactory _fairApiFactory;
    private readonly IWorkContext _workContext;
    private readonly IVendorService _vendorService;
    private readonly TimeProvider _timeProvider;

    public FairController(
        IFairService fairService,
        IFairApiFactory fairApiFactory,
        IWorkContext workContext,
        IVendorService vendorService,
        TimeProvider timeProvider)
    {
        _fairService = fairService;
        _fairApiFactory = fairApiFactory;
        _workContext = workContext;
        _vendorService = vendorService;
        _timeProvider = timeProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetFairsAsync(
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? untilUtc,
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageIndex = 0)
    {
        var fairs = await _fairService.GetAllFairsAsync(
            fromLocal: fromUtc ?? _timeProvider.GetUtcNow().LocalDateTime, //fairs are saved as local time. 
            untilLocal: untilUtc,
            pageSize: pageSize,
            pageIndex: pageIndex
            );

        var list = await _fairApiFactory.PrepareFairApiSlimModelListAsync(fairs);
        return ToJsonResult(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFairByIdAsync(int id)
    {
        var fair = await _fairService.GetFairByIdAsync(id);
        if (fair == null)
            return NotFound();

        var data = await _fairApiFactory.PrepareFairApiModelAsync(fair);
        return ToJsonResult(data);
    }

    [HttpPut("favorite")]
    public async Task<IActionResult> SetFairFavoriteAsync([FromBody] SetFairFavoriteRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == null)
            return Unauthorized();

        var fair = await _fairService.GetFairByIdAsync(request.FairId);
        if (fair == null)
            return NotFound();

        await _fairService.SetFairFavoriteAsync(customer, fair, value: request.IsFavorite);

        return Ok();
    }

    #region vendor
    [HttpGet("vendor/{mapId}")]
    public async Task<IActionResult> GetVendorFairInfoAsync(int mapId)
    {
        var map = await _fairService.GetFairVendorMapByIdAsync(mapId);
        if (map == default)
            return BadRequest();

        var data = await _fairApiFactory.PrepareFairVendorMapApiModelAsync(map, includeFairInfo: true, includeVendorProducts: true);
        if (data == default)
            return NotFound();
        return ToJsonResult(data);
    }
    #endregion
}