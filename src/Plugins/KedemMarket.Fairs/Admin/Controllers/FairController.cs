namespace KedemMarket.Fairs.Admin.Controllers;

public class FairController : BaseAdminController
{
    private const string VIEW_PATH = "~/Plugins/KedemMarket.Fairs/Admin/Views/Fairs/";

    private readonly IFairFactory _fairFactory;
    private readonly IFairService _fairService;
    private readonly IWorkContext _workContext;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    public FairController(
        IFairFactory fairFactory,
        IFairService fairService,
        IWorkContext workContext,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _fairFactory = fairFactory;
        _fairService = fairService;
        _workContext = workContext;
        _localizationService = localizationService;
        _notificationService = notificationService;
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

    [CheckPermission(FairPermissions.VIEW)]
    public virtual async Task<IActionResult> List([FromQuery] int offset = 0, [FromQuery] int pageSize = 25)
    {
        var model = new FairSearchModel();
        await _fairFactory.PrepareFairSearchModelAsync(model);
        return View("List.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FairPermissions.VIEW)]
    public virtual async Task<IActionResult> List(FairSearchModel searchModel)
    {
        var model = await _fairFactory.PrepareFairAdminListModelAsync(searchModel);
        return Json(model);
    }

    [CheckPermission(FairPermissions.CREATE)]
    public virtual async Task<IActionResult> Create()
    {
        var model = new FairAdminModel();
        await _fairFactory.PrepareFairAdminModelAsync(model, null);

        return View("Create.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.CREATE)]
    public virtual async Task<IActionResult> Create(FairAdminModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var fair = model.ToEntity<Fair>();
            fair.CreatedOnUtc = DateTime.UtcNow;

            await _fairService.InsertFairAsync(fair);

            var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Added");
            _notificationService.SuccessNotification(msg);

            if (!continueEditing || fair.Id == 0)
                return RedirectToAction(nameof(List));

            return RedirectToAction("Edit", new { id = fair.Id });
        }

        model = await _fairFactory.PrepareFairAdminModelAsync(model, null);
        return View("Create.cshtml", model);
    }


    [CheckPermission(FairPermissions.EDIT)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var fair = await _fairService.GetFairByIdAsync(id);
        if (fair == null || fair.Deleted)
            return RedirectToAction("List");

        var model = await _fairFactory.PrepareFairAdminModelAsync(null, fair);
        return View("Edit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.EDIT)]
    public virtual async Task<IActionResult> Edit(FairAdminModel model, bool continueEditing)
    {
        var fair = await _fairService.GetFairByIdAsync(model.Id);
        if (fair == null || fair.Deleted)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            fair.Name = model.Name;
            fair.Address = fair.IsVirtual ? null : model.Address.ToEntity<Address>();
            fair.StartsOnUtc = model.StartsOnUtc;
            fair.EndsOnUtc = model.EndsOnUtc;
            fair.Published = model.Published;
            fair.UpdatedOnUtc = DateTime.UtcNow;

            await _fairService.UpdateFairAsync(fair);

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
    [CheckPermission(FairPermissions.DELETE)]
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
    [CheckPermission(FairPermissions.EDIT)]
    [CheckPermission(FairPermissions.DELETE)]
    public virtual async Task<IActionResult> FairVendorList(FairVendorSearchModel searchModel)
    {
        var fair = await _fairService.GetFairByIdAsync(searchModel.FairId)
            ?? throw new ArgumentException("No fair info found with the specified id");

        var list = await _fairFactory.PrepareFairVendorListModelAsync(searchModel, fair);
        return Json(list);
    }

    [CheckPermission(FairPermissions.CREATE)]
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
    [CheckPermission(FairPermissions.CREATE)]
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

    [CheckPermission(FairPermissions.EDIT)]
    public virtual async Task<IActionResult> EditFairVendor(int id)
    {
        var fvm = await _fairService.GetFairVendorMapByIdAsync(id);
        var model = new CreateOrUpdateFairVendorModel
        {
            Id = id,
            VendorId = fvm.VendorId,
            FairId = fvm.FairId,
            DisplayOrder = fvm.DisplayOrder,
        };
        await _fairFactory.PrepareCreateOrUpdateFairVendorModelAsync(model);
        return View("Vendors/Edit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.EDIT)]
    public virtual async Task<IActionResult> EditFairVendor(CreateOrUpdateFairVendorModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var fvm = await _fairService.GetFairVendorMapByIdAsync(model.Id);
            fvm.DisplayOrder = model.DisplayOrder;

            await _fairService.UpdateFairVendorMapAsync(fvm);
            var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Vendors.Vendor.Updated");
            _notificationService.SuccessNotification(msg);
        }
        if (continueEditing)
            return RedirectToAction(nameof(EditFairVendor), new { id = model.Id });

        return RedirectToAction("Edit", new { id = model.FairId });
    }

    [HttpPost]
    [CheckPermission(FairPermissions.DELETE)]
    public virtual async Task<IActionResult> DeleteFairVendor(int id)
    {
        var fvm = await _fairService.GetFairVendorMapByIdAsync(id);
        await _fairService.DeleteFairVendorMapAsync(fvm);

        var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Vendors.Vendor.Delete");
        _notificationService.SuccessNotification(msg);
        return RedirectToAction("Edit", new { id = fvm.FairId });

    }
    #endregion
}