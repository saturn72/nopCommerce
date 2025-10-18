using Nop.Web.Framework.Components;

namespace KedemMarket.Brands.Components;
public class WidgetsAdminProductBrandViewComponent : NopViewComponent
{
    private readonly IBrandService _brandService;
    private readonly IBrandModelFactory _brandModelFactory;

    public WidgetsAdminProductBrandViewComponent(
        IBrandService brandService, 
        IBrandModelFactory brandModelFactory)
    {
        _brandService = brandService;
        _brandModelFactory = brandModelFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var pm = additionalData as ProductModel;
        if (pm.Id == 0)
            return Content("");

        var brands= await _brandService.GetProductBrandsByProductIdAsync(pm.Id);
        var model = await _brandModelFactory.PrepareBrandProductAdminModelsAsync(brands);

        return View($"{Consts.VIEW_PATH_BRAND_ADMIN}_CreateOrUpdate.Brands.cshtml", model);
    }
}
