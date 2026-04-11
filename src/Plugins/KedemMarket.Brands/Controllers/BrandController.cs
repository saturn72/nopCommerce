using Nop.Web.Controllers;

namespace KedemMarket.Brands.Controllers;

[AutoValidateAntiforgeryToken]
public class BrandController : BasePublicController
{
    private readonly IBrandModelFactory _brandModelFactory;
    private readonly IBrandService _brandService;

    public BrandController(
        IBrandModelFactory brandModelFactory,
        IBrandService brandService)
    {
        _brandModelFactory = brandModelFactory;
        _brandService = brandService;
    }

    public override ViewResult View(string viewName, object model) =>
        base.View(Consts.VIEW_PATH_BRAND + viewName, model);

    public virtual async Task<IActionResult> BrandDetails(int brandId)
    {
        var brand = await _brandService.GetBrandByIdAsync(brandId);
        if (brand == null)
            return InvokeHttp404();

        var model = await _brandModelFactory.PrepareBrandDetailsModelAsync(brand);

        return View("BrandDetails.cshtml", model);
    }
}