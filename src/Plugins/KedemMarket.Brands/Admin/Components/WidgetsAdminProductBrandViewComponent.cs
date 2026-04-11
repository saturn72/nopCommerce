using Nop.Web.Framework.Components;

namespace KedemMarket.Brands.Admin.Components;
public class WidgetsAdminProductBrandViewComponent : NopViewComponent
{
    private readonly IBrandService _brandService;
    private readonly IBrandModelAdminFactory _brandModelAdminFactory;

    public WidgetsAdminProductBrandViewComponent(
        IBrandService brandService, 
        IBrandModelAdminFactory brandModelAdminFactory)
    {
        _brandService = brandService;
        _brandModelAdminFactory = brandModelAdminFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var pm = additionalData as ProductModel;
        if (pm.Id == 0)
            return Content("");

        var brands= await _brandService.GetProductBrandsByProductIdAsync(pm.Id);
        var model = await _brandModelAdminFactory.PrepareBrandProductAdminModelsAsync(brands);

        return View($"{Consts.VIEW_PATH_BRAND_ADMIN}_CreateOrUpdate.Brands.cshtml", model);
    }
}
