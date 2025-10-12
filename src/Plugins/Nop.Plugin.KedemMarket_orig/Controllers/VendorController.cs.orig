<<<<<<< HEAD
﻿namespace KedemMarket.Controllers;
=======
﻿using KedemMarket.Services.Vendor;

namespace KedemMarket.Controllers;
>>>>>>> dev/get-vendors-sales

[Route("api/vendor")]
public class VendorController : KmApiControllerBase
{
    private readonly IVendorApiModelFactory _vendorApiModelFactory;
    private readonly IVendorService _vendorService;
<<<<<<< HEAD

    public VendorController(
        IVendorApiModelFactory vendorApiModelFactory,
        IVendorService vendorService)
    {
        _vendorApiModelFactory = vendorApiModelFactory;
        _vendorService = vendorService;
=======
    private readonly IWorkContext _workContext;
    private readonly Factories.Orders.IOrderModelFactory _orderModelFactory;
    private readonly IOrderService _orderService;
    private readonly IKmVendorService _kmVendorService;

    public VendorController(
        IVendorApiModelFactory vendorApiModelFactory,
        IVendorService vendorService,
        IWorkContext workContext,
        Factories.Orders.IOrderModelFactory orderModelFactory,
        IOrderService orderService,
        IKmVendorService kmVendorService)
    {
        _vendorApiModelFactory = vendorApiModelFactory;
        _vendorService = vendorService;
        _workContext = workContext;
        _orderModelFactory = orderModelFactory;
        _orderService = orderService;
        _kmVendorService = kmVendorService;
>>>>>>> dev/get-vendors-sales
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
<<<<<<< HEAD
=======

    [HttpGet("sales")]
    public async Task<IActionResult> GetVendorSalesAsync(
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0,
        [FromQuery] int[] orderStatus = null)
    {
        var vendor = await _workContext.GetCurrentVendorAsync();
        if (vendor == default)
            return BadRequest();

        var os = await _orderService.SearchOrdersAsync(
            vendorId: vendor.Id,
            pageIndex: offset / limit,
            pageSize: limit,
            osIds: orderStatus?.ToList());
        var data = await _orderModelFactory.PrepareVendorOrderModelsAsync(os, vendor);
        
        return ToJsonResult(data);
    }

    [HttpGet("sales/count")]
    public async Task<IActionResult> GetVendorOpenSalesCountAsync()
    {
        var vendor = await _workContext.GetCurrentVendorAsync();
        if (vendor == default)
            return BadRequest();

        var count = await _kmVendorService.GetVendorOpenOrdersCountAsync(vendor.Id);
        return ToJsonResult(new
        {
            total = count
        });
    }

    [HttpPut("sales/status")]
    public async Task<IActionResult> ChangeVendorOrderStatusAsync([FromBody] ChangeVendorOrderStatusRequest model)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var vendor = await _workContext.GetCurrentVendorAsync();
        if (vendor == default)
            return BadRequest();

        var order = await _orderService.GetOrderByIdAsync(model.OrderId);
        if (order == null)
            return BadRequest();

        var items = await _orderService.GetOrderItemsAsync(model.OrderId, vendorId: vendor.Id);
        if (items.Count == 0)
            return BadRequest();

        var orderItemIds = items.Select(i => i.Id).ToList();
        await _kmVendorService.SetOrdersItemStatusAsync(order, orderItemIds, model.Status);

        return NoContent();
    }
>>>>>>> dev/get-vendors-sales
}
