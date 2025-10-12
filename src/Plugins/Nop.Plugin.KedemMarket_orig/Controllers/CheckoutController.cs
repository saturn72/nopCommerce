namespace KedemMarket.Controllers;

[Route("api/checkout")]
public class CheckoutController : KmApiControllerBase
{
    private readonly IKmOrderService _kmOrderService;
    private readonly IShoppingCartFactory _shoppingCartFactory;
    private readonly IWorkContext _workContext;

    public CheckoutController(
        IKmOrderService kmOrderService,
        IShoppingCartFactory shoppingCartFactory,
        IWorkContext workContext)
    {
        _kmOrderService = kmOrderService;
        _shoppingCartFactory = shoppingCartFactory;
        _workContext = workContext;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitOrder([FromBody] CartTransactionApiModel model)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == default)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        var errors = new List<string>();
        var cor = await _shoppingCartFactory.ToCreateOrderRequest(model, errors);

        var res = await _kmOrderService.CreateOrderAsync(cor);
        if (res.IsError)
            return BadRequest(new { error = res.Error });

        var nopOrder = res.KmOrder.NopOrder;
        return Ok(new { id = nopOrder.Id, status = nopOrder.OrderStatus });
    }
}
