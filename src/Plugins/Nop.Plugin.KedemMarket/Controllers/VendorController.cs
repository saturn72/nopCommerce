
namespace KedemMarket.Controllers;

[Route("api/vendor")]
public class VendorController : KmApiControllerBase
{
    private readonly IVendorApiModelFactory _vendorApiModelFactory;
    private readonly IVendorService _vendorService;
    private readonly IWorkContext _workContext;
    private readonly IOrderApiModelFactory _orderModelFactory;
    private readonly IOrderService _orderService;

    public VendorController(
        IVendorApiModelFactory vendorApiModelFactory,
        IVendorService vendorService,
        IWorkContext workContext,
        IOrderApiModelFactory orderModelFactory,
        IOrderService orderService)
    {
        _vendorApiModelFactory = vendorApiModelFactory;
        _vendorService = vendorService;
        _workContext = workContext;
        _orderModelFactory = orderModelFactory;
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVendorsAsync()
    {
        var vendors = await _vendorApiModelFactory.GetAllVendorsAsync();
        return ToJsonResult(vendors);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVendorByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest();

        var vendor = await _vendorService.GetVendorByIdAsync(id);
        if (vendor == default)
            return NotFound();

        var data = await _vendorApiModelFactory.PrepareVendorApiModelAsync(vendor);
        return ToJsonResult(data);
    }

    [HttpGet("sales")]
    public async Task<IActionResult> GetVendorSalesAsync(int limit = 20, int offset = 0)
    {
        var vendor = await _workContext.GetCurrentVendorAsync();
        if (vendor == default)
            return BadRequest();

        var orders = await _orderService.SearchOrdersAsync(vendorId: vendor.Id, pageIndex: offset / limit, pageSize: limit);
        var data = await _orderModelFactory.PrepareOrderDetailsModelsAsync(orders);
        return ToJsonResult(data);
    }
}
