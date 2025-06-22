namespace KedemMarket.Controllers;

[Route("api/vendor")]
public class VendorController : KmApiControllerBase
{
    private readonly IVendorApiModelFactory _vendorApiModelFactory;
    private readonly IVendorService _vendorService;

    public VendorController(
        IVendorApiModelFactory vendorApiModelFactory,
        IVendorService vendorService)
    {
        _vendorApiModelFactory = vendorApiModelFactory;
        _vendorService = vendorService;
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
}
