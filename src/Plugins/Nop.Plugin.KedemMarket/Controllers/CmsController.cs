using Nop.Web.Areas.Admin.Models.Orders;

namespace KedemMarket.Controllers;

[Route("cms")]
public class CmsController : KmApiControllerBase
{
    private readonly IHomePageFactory _homePageFactory;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IEntityToModelFactory _entityToModelFactory;
    private readonly IOrderService _orderService;
    private readonly Nop.Web.Areas.Admin.Factories.IOrderModelFactory _orderModelFactory;

    public CmsController(
        IHomePageFactory homePageFactory,
        IWorkContext workContext,
        IUrlRecordService urlRecordService,
        IEntityToModelFactory entityToModelFactory,
        IOrderService orderService,
        Nop.Web.Areas.Admin.Factories.IOrderModelFactory orderModelFactory)
    {
        _homePageFactory = homePageFactory;
        _workContext = workContext;
        _urlRecordService = urlRecordService;
        _entityToModelFactory = entityToModelFactory;
        _orderService = orderService;
        _orderModelFactory = orderModelFactory;
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


    [HttpGet("sales")]
    public async Task<IActionResult> GetVendorSalesAsync(int limit = 20, int page = 0)
    {
        var vendor = await _workContext.GetCurrentVendorAsync();
        if (vendor == default)
            return BadRequest();

        var sm = new OrderSearchModel
        {
            VendorId = vendor.Id,
            Start = page,
            Length = limit,
        };
        //var orders = await _orderService.SearchOrdersAsync(vendorId: vendor.Id, pageIndex: offset / limit, pageSize: limit);

        var data = await _orderModelFactory.PrepareOrderListModelAsync(sm);
        return ToJsonResult(data.Data);
    }
}