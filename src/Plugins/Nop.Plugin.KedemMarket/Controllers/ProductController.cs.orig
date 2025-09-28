namespace KedemMarket.Controllers;
[Route("api/product")]
public class ProductController : KmApiControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductApiFactory _productApiFactory;
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;

    public ProductController(
        IProductService productService,
        IProductApiFactory productApiFactory,
        IStoreContext storeContext,
        IWorkContext workContext)
    {
        _productService = productService;
        _productApiFactory = productApiFactory;
        _storeContext = storeContext;
        _workContext = workContext;
    }

    [HttpGet]
    public async Task<IActionResult> Query(
        [FromQuery(Name = "q")] string keywords,
        [FromQuery] int vendorId = 0,
<<<<<<< HEAD
=======
        [FromQuery] int categoryId = 0,
>>>>>>> dev/get-vendors-sales
        [FromQuery] int offset = 0,
        [FromQuery] int pageSize = 50)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == default)
            return BadRequest();

        var store = await _storeContext.GetCurrentStoreAsync();
        var products = await _productService.SearchProductsAsync(
            offset,
            pageSize,
<<<<<<< HEAD
=======
            categoryIds: categoryId > 0?[categoryId]: null,
>>>>>>> dev/get-vendors-sales
            vendorId: vendorId,
            storeId: store.Id,
            keywords: keywords,
            searchDescriptions: true,
            searchManufacturerPartNumber: true,
            searchProductTags: true,
            searchSku: true,
            orderBy: ProductSortingEnum.Position
            );

        var data = await _productApiFactory.ToProductInfoApiModelAsync(products);
        return ToJsonResult(new { products = data });
    }
<<<<<<< HEAD

=======
>>>>>>> dev/get-vendors-sales
}
