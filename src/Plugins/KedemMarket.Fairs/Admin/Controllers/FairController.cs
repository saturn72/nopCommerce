using DocumentFormat.OpenXml.EMMA;
using KedemMarket.Fairs.Domain;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Controllers;

namespace KedemMarket.Fairs.Admin.Controllers;
[AutoValidateAntiforgeryToken]
public class FairController : BaseAdminController
{
    private const string VIEW_PATH = "~/Plugins/KedemMarket.Fairs/Admin/Views/Fairs/";

    private readonly IFairFactory _fairFactory;
    private readonly IFairService _fairService;
    private readonly IWorkContext _workContext;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly TimeProvider _timeProvider;
    private readonly IPictureService _pictureService;
    private readonly IVendorService _vendorService;
    private readonly IProductService _productService;

    public FairController(
        IFairFactory fairFactory,
        IFairService fairService,
        IWorkContext workContext,
        ILocalizationService localizationService,
        INotificationService notificationService,
        TimeProvider timeProvider,
        IPictureService pictureService,
        IVendorService vendorService,
        IProductService productService)
    {
        _fairFactory = fairFactory;
        _fairService = fairService;
        _workContext = workContext;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _timeProvider = timeProvider;
        _pictureService = pictureService;
        _vendorService = vendorService;
        _productService = productService;
    }
    public static string GetViewPath(string viewName) => VIEW_PATH + viewName;
    public override ViewResult View(string viewName, object model)
    {
        return base.View(GetViewPath(viewName), model);
    }

    #region Fair
    public virtual IActionResult Index([FromQuery] int offset = 0, [FromQuery] int pageSize = 25)
    {
        return RedirectToAction(nameof(List), new { offset, pageSize });
    }

    [CheckPermission(FairPermissions.ADMIN_VIEW)]
    public virtual async Task<IActionResult> List([FromQuery] int offset = 0, [FromQuery] int pageSize = 25)
    {
        var model = new FairSearchModel();
        await _fairFactory.PrepareFairSearchModelAsync(model);
        return View("List.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FairPermissions.ADMIN_VIEW)]
    public virtual async Task<IActionResult> List(FairSearchModel searchModel)
    {
        var vendor = await _workContext.GetCurrentVendorAsync();
        if (vendor != default)
            searchModel.VendorIds = [vendor.Id];

        var model = await _fairFactory.PrepareFairAdminListModelAsync(searchModel);
        return Json(model);
    }

    [CheckPermission(FairPermissions.ADMIN_CREATE)]
    public virtual async Task<IActionResult> Create()
    {
        var model = new FairAdminModel();
        await _fairFactory.PrepareFairAdminModelAsync(model, null);

        return View("Create.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.ADMIN_CREATE)]
    public virtual async Task<IActionResult> Create(FairAdminModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var fair = model.ToEntity<Fair>();
            fair.CreatedOnUtc = DateTime.UtcNow;

            await _fairService.InsertFairAsync(fair);

            await UpdatePictureSeoNamesAsync(fair);

            var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Added");
            _notificationService.SuccessNotification(msg);

            if (!continueEditing || fair.Id == 0)
                return RedirectToAction(nameof(List));

            return RedirectToAction("Edit", new { id = fair.Id });
        }

        model = await _fairFactory.PrepareFairAdminModelAsync(model, null);
        return View("Create.cshtml", model);
    }


    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var fair = await _fairService.GetFairByIdAsync(id);
        if (fair == null || fair.Deleted)
            return RedirectToAction("List");

        var model = await _fairFactory.PrepareFairAdminModelAsync(null, fair);
        return View("Edit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    public virtual async Task<IActionResult> Edit(FairAdminModel model, bool continueEditing)
    {
        var fair = await _fairService.GetFairByIdAsync(model.Id);
        if (fair == null || fair.Deleted)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            var prevPictureId = fair.PictureId;
            fair = model.ToEntity(fair);
            fair.UpdatedOnUtc = _timeProvider.GetUtcNow().UtcDateTime;

            await _fairService.UpdateFairAsync(fair);
            if (prevPictureId > 0 && prevPictureId != fair.PictureId)
            {
                var prevPicture = await _pictureService.GetPictureByIdAsync(prevPictureId);
                if (prevPicture != null)
                    await _pictureService.DeletePictureAsync(prevPicture);
            }

            await UpdatePictureSeoNamesAsync(fair);

            var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Updated");
            _notificationService.SuccessNotification(msg);

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = fair.Id });
        }

        model = await _fairFactory.PrepareFairAdminModelAsync(model, fair);
        return View("Edit.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FairPermissions.ADMIN_DELETE)]
    public virtual async Task<IActionResult> DeleteFair(int id)
    {
        var fair = await _fairService.GetFairByIdAsync(id);

        if (fair == null)
            return NotFound();

        await _fairService.DeleteFairAsync(fair);
        return RedirectToAction(nameof(List));
    }

    #endregion

    #region Vendors    
    [HttpPost]
    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    [CheckPermission(FairPermissions.ADMIN_DELETE)]
    public virtual async Task<IActionResult> FairVendorList(FairVendorSearchModel searchModel)
    {
        var fair = await _fairService.GetFairByIdAsync(searchModel.FairId)
            ?? throw new ArgumentException("No fair info found with the specified id");

        var list = await _fairFactory.PrepareFairVendorListModelAsync(searchModel, fair);
        return Json(list);
    }

    [CheckPermission(FairPermissions.ADMIN_CREATE)]
    public virtual async Task<IActionResult> FairVendorCreate(int fairId)
    {
        var model = new CreateOrUpdateFairVendorModel
        {
            FairId = fairId,
        };
        await _fairFactory.PrepareCreateOrUpdateFairVendorModelAsync(model);

        return View("Vendors/Create.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.ADMIN_CREATE)]
    public virtual async Task<IActionResult> CreateFairVendor(CreateOrUpdateFairVendorModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var fvm = model.ToEntity<FairVendorMap>();
            await _fairService.InsertFairVendorMapAsync(fvm);
            var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Vendors.Vendor.Added");
            _notificationService.SuccessNotification(msg);
            if (continueEditing)
                return RedirectToAction(nameof(EditFairVendor), new { id = fvm.Id });
        }

        return RedirectToAction(nameof(Edit), new { id = model.FairId });
    }

    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    public virtual async Task<IActionResult> EditFairVendor(int id)
    {
        var fvm = await _fairService.GetFairVendorMapByIdAsync(id);
        var model = new CreateOrUpdateFairVendorModel
        {
            Id = id,
            VendorId = fvm.VendorId,
            FairId = fvm.FairId,
            DisplayOrder = fvm.DisplayOrder,
            AutoApproveProducts = fvm.AutoApproveProducts,
        };
        await _fairFactory.PrepareCreateOrUpdateFairVendorModelAsync(model);
        return View("Vendors/Edit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    public virtual async Task<IActionResult> EditFairVendor(CreateOrUpdateFairVendorModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var fvm = await _fairService.GetFairVendorMapByIdAsync(model.Id);
            fvm.DisplayOrder = model.DisplayOrder;
            fvm.AutoApproveProducts = model.AutoApproveProducts;

            await _fairService.UpdateFairVendorMapAsync(fvm);
            var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Vendors.Vendor.Updated");
            _notificationService.SuccessNotification(msg);
        }
        if (continueEditing)
            return RedirectToAction(nameof(EditFairVendor), new { id = model.Id });

        return RedirectToAction("Edit", new { id = model.FairId });
    }

    [HttpPost]
    [CheckPermission(FairPermissions.ADMIN_DELETE)]
    public virtual async Task<IActionResult> DeleteFairVendor(int id)
    {
        var fvm = await _fairService.GetFairVendorMapByIdAsync(id);
        await _fairService.DeleteFairVendorMapAsync(fvm);

        var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Vendors.Vendor.Delete");
        _notificationService.SuccessNotification(msg);
        return RedirectToAction("Edit", new { id = fvm.FairId });

    }

    [HttpPost]
    [CheckPermission(FairPermissions.ADMIN_VIEW)]
    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    public virtual async Task<IActionResult> GetFairVendorProductList(FairVendorProductSearchModel searchModel)
    {
        searchModel.Fair = await _fairService.GetFairByIdAsync(searchModel.FairId);
        if (searchModel.Fair == null)
            return BadRequest();

        searchModel.Vendor = await _vendorService.GetVendorByIdAsync(searchModel.VendorId);
        if (searchModel.Vendor == null)
            return BadRequest();

        var data = await _fairFactory.PrepareFairVendorProductListModelAsync(searchModel);
        return Json(data);
    }

    [CheckPermission(FairPermissions.ADMIN_VIEW)]
    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    public virtual async Task<IActionResult> FairVendorProductAddPopup(int fairId, int vendorId)
    {
        var model = new AddProductToFairVendorSearchModel
        {
            FairId = fairId,
            VendorId = vendorId
        };
        await _fairFactory.PrepareAddProductToFairVendorSearchModelAsync(model);

        return View("Vendors/FairVendorProductAddPopup.cshtml", model);
    }
    [HttpPost]
    [CheckPermission(FairPermissions.ADMIN_VIEW)]
    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    public virtual async Task<IActionResult> FairVendorProductAddPopupList(AddProductToFairVendorSearchModel searchModel)
    {
        //prepare model
        var model = await _fairFactory.PrepareAddFairVendorProductAddPopupListAsync(searchModel);
        return Json(model);
    }

    [HttpPost]
    [FormValueRequired("save")]
    [CheckPermission(FairPermissions.ADMIN_VIEW)]
    [CheckPermission(FairPermissions.ADMIN_EDIT)]
    public virtual async Task<IActionResult> FairVendorProductAddPopup(AddProductToFairVendorModel model)
    {
        var errorModel = new AddProductToFairVendorSearchModel
        {
            FairId = model.FairId,
            VendorId = model.VendorId
        };

        var vendor = await _vendorService.GetVendorByIdAsync(model.VendorId);
        if (vendor == default)
            return await FairVendorProductAddPopupErrorViewAsync(errorModel, "Admin.Fairs.AddProductToFairVendorModel.InvalidVendor");

        var fair = await _fairService.GetFairByIdAsync(model.FairId);
        if (fair == null)
            return await FairVendorProductAddPopupErrorViewAsync(errorModel, "Admin.Fairs.AddProductToFairVendorModel.InvalidFair");

        var productIds = model.SelectedProductIds.ToArray();
        var vendors = await _vendorService.GetVendorsByProductIdsAsync(productIds);
        if (vendors?.Count != 1 || vendors.FirstOrDefault() != vendor)
            return await FairVendorProductAddPopupErrorViewAsync(errorModel, "Admin.Fairs.AddProductToFairVendorModel.InvalidVendor");

        var selectedProducts = await _productService.GetProductsByIdsAsync(productIds);
        if (selectedProducts.Any())
        {
            var maps = await _fairService.GetFairVendorProductMapsAsync(fair, vendor);
            foreach (var product in selectedProducts)
            {
                if (maps.FirstOrDefault(m => m.ProductId == product.Id) != null)
                    continue;

                //insert the new product category mapping
                await _fairService.InserFairVendorProductMapAsync(new FairVendorProductMap
                {
                    FairId = fair.Id,
                    VendorId = vendor.Id,
                    ProductId = product.Id,
                });
            }
        }

        ViewBag.RefreshPage = true;
        var m = new AddProductToFairVendorSearchModel
        {
            FairId = model.FairId,
            VendorId = model.VendorId
        };
        await _fairFactory.PrepareAddProductToFairVendorSearchModelAsync(m);
        return View("Vendors/FairVendorProductAddPopup.cshtml", m);
    }

    private async Task<IActionResult> FairVendorProductAddPopupErrorViewAsync(AddProductToFairVendorSearchModel model, string resourceKey)
    {
        await _fairFactory.PrepareAddProductToFairVendorSearchModelAsync(model);
        var msg = await _localizationService.GetResourceAsync(resourceKey);
        _notificationService.ErrorNotification(msg);

        return View("Vendors/FairVendorProductAddPopup.cshtml", model);
    }

    #endregion

    protected virtual async Task UpdatePictureSeoNamesAsync(Fair fair)
    {
        var picture = await _pictureService.GetPictureByIdAsync(fair.PictureId);
        if (picture != null)
            await _pictureService.SetSeoFilenameAsync(picture.Id, await _pictureService.GetPictureSeNameAsync(fair.Name));
    }
}