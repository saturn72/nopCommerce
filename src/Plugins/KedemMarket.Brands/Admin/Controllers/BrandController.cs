using KedemMarket.Brands.Admin.Models;
using KedemMarket.Brands.Security;
using Microsoft.AspNetCore.Http;
using Nop.Services.Localization;
using Nop.Services.Messages;

namespace KedemMarket.Brands.Admin.Controllers;

[AutoValidateAntiforgeryToken]
[AuthorizeAdmin] //confirms access to the admin panel
[Area(AreaNames.ADMIN)] //specifies the area containing a controller or action
public class BrandController : BaseAdminController
{
    private readonly IBrandModelAdminFactory _brandModelAdminFactory;
    private readonly INotificationService _notificationService;
    private readonly ILocalizationService _localizationService;
    private readonly IBrandImportExportManager _importExport;

    public BrandController(
        IBrandModelAdminFactory brandModelAdminFactory,
        INotificationService notificationService,
        ILocalizationService localizationService,
        IBrandImportExportManager importExport)
    {
        _brandModelAdminFactory = brandModelAdminFactory;
        _notificationService = notificationService;
        _localizationService = localizationService;
        _importExport = importExport;
    }

    public override ViewResult View(string viewName, object model) =>
        base.View(Consts.VIEW_PATH_BRAND_ADMIN + viewName, model);

    public virtual IActionResult Index() =>
        RedirectToAction(nameof(List));

    [CheckPermission(BrandsPermissions.BRANDS_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        var model = await _brandModelAdminFactory.PrepareBrandSearchModelAsync(new BrandSearchModel());
        return View("List.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(BrandsPermissions.BRANDS_VIEW)]
    public virtual async Task<IActionResult> BrandList(BrandSearchModel searchModel)
    {
        var model = await _brandModelAdminFactory.PrepareBrandListModelAsync(searchModel);
        return Json(model);
    }

    [CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create()
    {
        var model = await _brandModelAdminFactory.PrepareBrandProductAdminModelAsync(null, new Brand());
        return View("Create.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> ImportJson(IFormFile importjsonfile)
    {
        try
        {
            await _importExport.ImportFromJsonAsync(importjsonfile.OpenReadStream());
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Brands.ImportSuccess"));
        }
        catch (Exception ex)
        {
            await _notificationService.ErrorNotificationAsync(ex);
        }

        return RedirectToAction(nameof(List));
    }

    //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")
    //    [CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> Create(BrandModel model, bool continueEditing)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var navbar = model.ToEntity<Brand>();
    //        navbar.CreatedOnUtc = DateTime.UtcNow;
    //        navbar.UpdatedOnUtc = DateTime.UtcNow;
    //        await _brandService.InsertBrandAsync(navbar);

    //        var msg = await _localizationService.GetResourceAsync("Admin.Navbars.Added");
    //        _notificationService.SuccessNotification(msg);

    //        await SaveStoreMappingsAsync(navbar, model);
    //        if (!continueEditing || navbar.Id == 0)
    //            return RedirectToAction("List");

    //        return RedirectToAction("Edit", new { id = navbar.Id });
    //    }

    //    model = await _navbarFactory.PrepareBrandModelAsync(model, null, true);
    //    return View("Create.cshtml", model);
    //}

    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> Edit(int id)
    //{
    //    var navbar = await _brandService.GetBrandByIdAsync(id);
    //    if (navbar == null || navbar.Deleted)
    //        return RedirectToAction("List");

    //    //prepare model
    //    var model = await _navbarFactory.PrepareBrandModelAsync(null, navbar);

    //    return View("Edit.cshtml", model);
    //}

    //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> Edit(BrandModel model, bool continueEditing)
    //{
    //    //try to get a brand with the specified id
    //    var navbar = await _brandService.GetBrandByIdAsync(model.Id);
    //    if (navbar == null || navbar.Deleted)
    //        return RedirectToAction("List");

    //    if (ModelState.IsValid)
    //    {
    //        navbar.UpdatedOnUtc = DateTime.UtcNow;
    //        navbar.Name = model.Name;


    //        navbar = model.ToEntity(navbar);
    //        navbar.UpdatedOnUtc = DateTime.UtcNow;
    //        await _brandService.UpdateBrandAsync(navbar);

    //        await SaveStoreMappingsAsync(navbar, model);

    //        var msg = await _localizationService.GetResourceAsync("Admin.Navbars.Updated");
    //        _notificationService.SuccessNotification(msg);

    //        if (!continueEditing)
    //            return RedirectToAction("List");

    //        return RedirectToAction("Edit", new { id = navbar.Id });
    //    }

    //    model = await _navbarFactory.PrepareBrandModelAsync(model, navbar, true);
    //    return View("Edit.cshtml", model);
    //}

    //protected virtual async Task SaveStoreMappingsAsync(Brand brand, BrandModel model)
    //{
    //    brand.LimitedToStores = model.SelectedStoreIds.Any();
    //    await _brandService.UpdateBrandAsync(brand);

    //    var existingStoreMappings = await _storeMappingService.GetStoreMappingsAsync(brand);
    //    var allStores = await _storeService.GetAllStoresAsync();
    //    foreach (var store in allStores)
    //        if (model.SelectedStoreIds.Contains(store.Id))
    //            //new store
    //            if (!existingStoreMappings.Any(sm => sm.StoreId == store.Id))
    //                await _storeMappingService.InsertStoreMappingAsync(brand, store.Id);
    //            else
    //            {
    //                //remove store
    //                var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
    //                if (storeMappingToDelete != null)
    //                    await _storeMappingService.DeleteStoreMappingAsync(storeMappingToDelete);
    //            }
    //}

    //[HttpPost]
    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> Delete(int id)
    //{
    //    //try to get a brand with the specified id
    //    var navbar = await _brandService.GetBrandByIdAsync(id);
    //    if (navbar == null)
    //        return RedirectToAction("List");

    //    await DeleteStoreMappingsAsync(navbar);
    //    await _brandService.DeleteBrandsAsync([navbar]);
    //    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Navbars.Deleted"));

    //    return RedirectToAction("List");
    //}

    //[HttpPost]
    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> DeleteSelected(ICollection<int> selectedIds)
    //{
    //    if (selectedIds == null || !selectedIds.Any())
    //        return NoContent();

    //    var navbars = await _brandService.GetBrandByIdsAsync(selectedIds.ToArray());
    //    var tasks = new List<Task>();
    //    foreach (var navbar in navbars)
    //        tasks.Add(DeleteStoreMappingsAsync(navbar));
    //    await Task.WhenAll(tasks);

    //    await _brandService.DeleteBrandsAsync(navbars);

    //    return Json(new { Result = true });
    //}

    //protected virtual async Task DeleteStoreMappingsAsync(Brand brand)
    //{
    //    var existingStoreMappings = await _storeMappingService.GetStoreMappingsAsync(brand);
    //    var allStores = await _storeService.GetAllStoresAsync();
    //    foreach (var store in allStores)
    //    {
    //        var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
    //        if (storeMappingToDelete != null)
    //            await _storeMappingService.DeleteStoreMappingAsync(storeMappingToDelete);
    //    }
    //}

    //#region Elements    
    //[HttpPost]
    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //[CheckPermission(NavbarPermissions.NAVBARS_ELEMENTS_DELETE)]
    //public virtual async Task<IActionResult> NavbarElementList(NavbarElementSearchModel searchModel)
    //{
    //    var navbar = await _brandService.GetBrandByIdAsync(searchModel.BrandId)
    //        ?? throw new ArgumentException("No navbar info found with the specified id");

    //    var model = await _navbarFactory.PrepareBrandElementListModelAsync(searchModel, navbar);

    //    return Json(model);
    //}

    //[CheckPermission(NavbarPermissions.NAVBARS_ELEMENTS_CREATE)]
    //public virtual async Task<IActionResult> NavbarElementCreate(int brandId)
    //{
    //    var model = new CreateOrUpdateNavbarElementModel
    //    {
    //        BrandId = brandId
    //    };
    //    await _navbarFactory.PrepareCreateOrUpdateNavbarElementModelAsync(model);

    //    return View("NavbarElement/Create.cshtml", model);
    //}

    //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    //[CheckPermission(NavbarPermissions.NAVBARS_ELEMENTS_CREATE)]
    //public virtual async Task<IActionResult> NavbarElementCreate(CreateOrUpdateNavbarElementModel model, bool continueEditing)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var navbarElement = model.ToEntity<NavbarElement>();
    //        await _brandService.InsertNavbarElementAsync(navbarElement);
    //        var msg = await _localizationService.GetResourceAsync("Admin.Navbars.Elements.Added");
    //        _notificationService.SuccessNotification(msg);
    //        if (continueEditing)
    //            return RedirectToAction(nameof(EditNavbarElement), new { id = model.BrandId });
    //        return RedirectToAction("Edit", new { id = model.BrandId });
    //    }

    //    await _navbarFactory.PrepareCreateOrUpdateNavbarElementModelAsync(model);
    //    return View("NavbarElement/Create.cshtml", model);
    //}

    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> EditNavbarElement(int id)
    //{
    //    var e = await _brandService.GetNavbarElementsByIdAsync(id);
    //    var model = e.ToModel<CreateOrUpdateNavbarElementModel>();
    //    model.NavbarElementId = id;
    //    await _navbarFactory.PrepareCreateOrUpdateNavbarElementModelAsync(model);
    //    return View("NavbarElement/Edit.cshtml", model);
    //}

    //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> EditNavbarElement(CreateOrUpdateNavbarElementModel model, bool continueEditing)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var navbarElement = model.ToEntity<NavbarElement>();
    //        await _brandService.UpdateNavbarElementAsync(navbarElement);
    //        var msg = await _localizationService.GetResourceAsync("Admin.Navbars.Elements.Updated");
    //        _notificationService.SuccessNotification(msg);
    //    }
    //    if (continueEditing)
    //        return RedirectToAction(nameof(EditNavbarElement), new { id = model.Id });
    //    return RedirectToAction("Edit", new { id = model.BrandId });
    //}

    //[HttpPost]
    //[CheckPermission(NavbarPermissions.NAVBARS_ELEMENTS_DELETE)]
    //public virtual async Task<IActionResult> DeleteNavbarElement(Models.Navbar.NavbarElementModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var navbarElement = model.ToEntity<NavbarElement>();
    //        await _brandService.DeleteNavbarElementAsync(navbarElement);
    //        var msg = await _localizationService.GetResourceAsync("Admin.Navbars.Elements.Deleted");
    //        _notificationService.SuccessNotification(msg);
    //    }
    //    return new NullJsonResult();
    //}

    //#endregion

    //#region Vendors
    //[HttpPost]
    //[CheckPermission(NavbarPermissions.NAVBARS_ELEMENTS_VIEW)]
    //public virtual async Task<IActionResult> NavbarElementVendorList(NavbarElementVendorListSearchModel searchModel)
    //{
    //    var model = await _navbarFactory.PrepareNavbarElementVendorListSearchModelAsync(searchModel);
    //    return Json(model);
    //}

    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> VendorAddPopup(int navbarElementId)
    //{
    //    var searchModel = new AddVendorToNavbarElementSearchModel
    //    {
    //        NavbarElementId = navbarElementId
    //    };
    //    await _navbarFactory.PrepareAddVendorToNavbarElementModel(searchModel);

    //    return View("NavbarElement/VendorAddPopup.cshtml", searchModel);
    //}
    //[HttpPost]
    //[CheckPermission(NavbarPermissions.NAVBARS_ELEMENTS_VIEW)]
    //public virtual async Task<IActionResult> VendorAddPopupList(NavbarElementVendorListSearchModel searchModel)
    //{
    //    var model = await _navbarFactory.VendorAddPopupListAsync(searchModel);
    //    return Json(model);
    //}

    //[HttpPost]
    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public virtual async Task<IActionResult> VendorAddPopup(AddVendorToNavbarElementSearchModel model)
    //{
    //    await _brandService.AddNavbarElementVendorsAsync(model.NavbarElementId, model.SelectedVendorIds);
    //    ViewBag.RefreshPage = true;
    //    var msg = await _localizationService.GetResourceAsync("Admin.Navbars.Elements.Vendors.Deleted");
    //    _notificationService.SuccessNotification(msg);
    //    return View("NavbarElement/VendorAddPopup.cshtml", new AddVendorToNavbarElementSearchModel());
    //}

    //[HttpPost]
    //[CheckPermission(NavbarPermissions.NAVBARS_ELEMENTS_DELETE)]
    //public virtual async Task<IActionResult> DeleteNavbarElementVendor(int id)
    //{
    //    if (id <= 0)
    //        return new NullJsonResult();

    //    var nev = await _brandService.GetNavbarElementVendorByIdAsync(id);
    //    await _brandService.DeleteNavbarElementVendorAsync(nev);
    //    var msg = await _localizationService.GetResourceAsync("Admin.Navbars.Elements.Vendors.Added");
    //    _notificationService.SuccessNotification(msg);
    //    ViewBag.RefreshPage = true;
    //    return new NullJsonResult();
    //}


    //[CheckPermission(BrandsPermissions.BRANDS_CREATE_EDIT_DELETE)]
    //public async Task<IActionResult> UpdateNavbarElementVendor(NavbarElementVendorModel model)
    //{
    //    var nev = await _brandService.GetNavbarElementVendorByIdAsync(model.Id)
    //        ?? throw new ArgumentException("No navbar element to vendor mapping found with the specified id");

    //    nev.DisplayOrder = model.DisplayOrder;
    //    nev.IsFeaturedVendor = model.IsFeaturedVendor;
    //    nev.Published = model.Published;
    //    nev.PublishPhone = model.PublishPhone;
    //    nev.PublishWhatsapp = model.PublishWhatsapp;
    //    await _brandService.UpdateNavbarElementVendorAsync(nev);

    //    return new NullJsonResult();
    //}
    //#endregion
}