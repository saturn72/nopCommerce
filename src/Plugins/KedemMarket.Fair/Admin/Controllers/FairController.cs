
namespace KedemMarket.Fair.Admin.Controllers;

public class FairController : BaseAdminController
{
    private const string VIEW_PATH = "~/Plugins/KedemMarket.Fair/Admin/Views/Fair/";

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

    [CheckPermission(FairPermissions.FAIRS_VIEW)]
    public virtual async Task<IActionResult> List([FromQuery] int offset = 0, [FromQuery] int pageSize = 25)
    {
        var model = new FairInfoSearchModel();
        await _fairFactory.PrepareFairInfoSearchModelAsync(model);
        return View("List.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FairPermissions.FAIRS_VIEW)]
    public virtual async Task<IActionResult> List(FairInfoSearchModel searchModel)
    {
        var model = await _fairFactory.PrepareFairInfoListModelAsync(searchModel);
        return Json(model);
    }


    [CheckPermission(FairPermissions.FAIRS_CREATE)]
    public virtual async Task<IActionResult> Create()
    {
        var model = new FairInfoAdminModel();
        await _fairFactory.PrepareFairInfoAdminModelAsync(model, null);

        return View("Create.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.FAIRS_CREATE)]
    public virtual async Task<IActionResult> Create(FairInfoAdminModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var fair = model.ToEntity<FairInfo>();
            fair.CreatedOnUtc = DateTime.UtcNow;

            await _fairService.InsertFairInfoAsync(fair);

            var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Added");
            _notificationService.SuccessNotification(msg);

            if (!continueEditing || fair.Id == 0)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = fair.Id });
        }

        model = await _fairFactory.PrepareFairInfoAdminModelAsync(model, null);
        return View("Create.cshtml", model);
    }


    [CheckPermission(FairPermissions.FAIRS_EDIT)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var fair = await _fairService.GetFairInfoByIdAsync(id);
        if (fair == null || fair.Deleted)
            return RedirectToAction("List");

        var model = await _fairFactory.PrepareFairInfoAdminModelAsync(null, fair);
        return View("Edit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.FAIRS_EDIT)]
    public virtual async Task<IActionResult> Edit(FairInfoAdminModel model, bool continueEditing)
    {
        var fair = await _fairService.GetFairInfoByIdAsync(model.Id);
        if (fair == null || fair.Deleted)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            fair.UpdatedOnUtc = DateTime.UtcNow;
            fair.Name = model.Name;


            fair = model.ToEntity(fair);
            fair.UpdatedOnUtc = DateTime.UtcNow;
            await _fairService.UpdateFairInfoAsync(fair);

            var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Updated");
            _notificationService.SuccessNotification(msg);

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = fair.Id });
        }

        model = await _fairFactory.PrepareFairInfoAdminModelAsync(model, fair);
        return View("Edit.cshtml", model);
    }

    #endregion

    #region Vendors    
    [HttpPost]
    [CheckPermission(FairPermissions.FAIRS_EDIT)]
    [CheckPermission(FairPermissions.FAIRS_DELETE)]
    public virtual async Task<IActionResult> FairVendorList(FairVendorSearchModel searchModel)
    {
        var fair = await _fairService.GetFairInfoByIdAsync(searchModel.FairInfoId)
            ?? throw new ArgumentException("No fair info found with the specified id");

        var list = await _fairFactory.PrepareFairInfoVendorListModelAsync(searchModel, fair);
        return Json(list);
    }
    [CheckPermission(FairPermissions.FAIRS_EDIT)]
    public virtual async Task<IActionResult> EditFairVendor(int id)
    {
        throw new NotImplementedException();
        //var e = await _fairService.GetFairVendorsByIdAsync(id);
        //var model = e.ToModel<CreateOrUpdateFairVendorModel>();
        //model.FairVendorId = id;
        //await _fairFactory.PrepareCreateOrUpdateFairVendorModelAsync(model);
        //return View("FairVendor/Edit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FairPermissions.FAIRS_EDIT)]
    public virtual async Task<IActionResult> EditFairVendor(object/*CreateOrUpdateFairVendorModel */model, bool continueEditing)
    {
        throw new NotImplementedException();
        //if (ModelState.IsValid)
        //{
        //    var fairVendor = model.ToEntity<FairVendor>();
        //    await _fairInfoService.UpdateFairVendorAsync(fairVendor);
        //    var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Vendors.Updated");
        //    _notificationService.SuccessNotification(msg);
        //}
        //if (continueEditing)
        //    return RedirectToAction(nameof(EditFairVendor), new { id = model.Id });
        //return RedirectToAction("Edit", new { id = model.FairInfoId });
    }

    [HttpPost]
    [CheckPermission(FairPermissions.FAIRS_DELETE)]
    public virtual async Task<IActionResult> DeleteFairVendor(FairVendorModel model)
    {
        throw new NotImplementedException();
        //if (ModelState.IsValid)
        //{
        //    var fairVendor = model.ToEntity<FairVendor>();
        //    await _fairInfoService.DeleteFairVendorAsync(fairVendor);
        //    var msg = await _localizationService.GetResourceAsync("Admin.Fairs.Vendor.Deleted");
        //    _notificationService.SuccessNotification(msg);
        //}
        //return new NullJsonResult();
    }
    #endregion
}