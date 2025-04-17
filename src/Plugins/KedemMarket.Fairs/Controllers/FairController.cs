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

    public FairController(
        IFairService fairService,
        IFairApiFactory fairApiFactory,
        IWorkContext workContext)
    {
        _fairService = fairService;
        _fairApiFactory = fairApiFactory;
        _workContext = workContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetFairsAsync(
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? untilUtc,
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageIndex = 0)
    {
        var fairs = await _fairService.GetAllFairsAsync(
            fromUtc: fromUtc,
            untilUtc: untilUtc,
            pageSize: pageSize,
            pageIndex: pageIndex
            );

        var list = await _fairApiFactory.PrepareFairApiModelListAsync(fairs);
        return ToJsonResult(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFairByIdAsync(int id)
    {
        var fair = await _fairService.GetFairByIdAsync(id);
        if (fair == null)
            return NotFound();

        var vendors = await _fairService.GetVendorsByFairIdAsync(id);

        var data = await _fairApiFactory.PrepareFairApiModelAsync(fair, vendors);
        return ToJsonResult(data);
    }

    [HttpPut("favorite")]
    public async Task<IActionResult> SetFairFavoriteAsync([FromBody]SetFairFavoriteRequest request)
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
}