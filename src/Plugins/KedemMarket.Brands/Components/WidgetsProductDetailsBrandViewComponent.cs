using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace KedemMarket.Brands.Components;
public class WidgetsProductDetailsBrandViewComponent : NopViewComponent
{
    private readonly IBrandModelFactory _brandFactory;

    public WidgetsProductDetailsBrandViewComponent(
        IBrandModelFactory brandFactory)
    {
        _brandFactory = brandFactory;
    }
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var pm = additionalData as ProductDetailsModel;
        if (pm == null || pm.Id == 0)
            return Content("");

        var model = await _brandFactory.PrepareProductBrandProductModelsAsync(pm.Id);
        return View($"{Consts.VIEW_PATH_BRAND}_ProductDetails.Brands.cshtml", model);
    }
}
