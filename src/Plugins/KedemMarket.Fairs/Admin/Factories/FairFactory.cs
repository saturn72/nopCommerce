using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace KedemMarket.Fairs.Admin.Factories;

public class FairFactory : IFairFactory
{
    private readonly IFairService _fairService;
    private readonly FairSettings _fairSettings;
    private readonly TimeProvider _timeProvider;
    private readonly IBaseAdminModelFactory _baseAdminModelFactory;
    private readonly IVendorService _vendorService;
    private readonly IProductService _productService;
    private readonly FairAdminSettings _fairAdminSettings;

    public FairFactory(
        IFairService fairService,
        FairSettings fairSettings,
        TimeProvider timeProvider,
        IBaseAdminModelFactory baseAdminModelFactory,
        IVendorService vendorService,
        IProductService productService,
        FairAdminSettings fairAdminSettings)
    {
        _fairService = fairService;
        _fairSettings = fairSettings;
        _timeProvider = timeProvider;
        _baseAdminModelFactory = baseAdminModelFactory;
        _vendorService = vendorService;
        _productService = productService;
        _fairAdminSettings = fairAdminSettings;
    }
    public Task PrepareFairSearchModelAsync(FairSearchModel searchModel)
    {
        searchModel.SetGridPageSize();
        return Task.CompletedTask;
    }

    public Task<FairAdminModel> PrepareFairAdminModelAsync(FairAdminModel model, Fair fair)
    {
        if (fair != null)
        {
            if (model == null)
                model = fair.ToModel<FairAdminModel>();
        }

        model ??= new FairAdminModel();
        if (fair == null)
        {
            model.Published = false;
            model.Deleted = false;
            model.StartsOnLocalDateTime = _timeProvider.GetUtcNow().LocalDateTime;
            model.EndsOnLocalDateTime = _timeProvider.GetUtcNow().LocalDateTime.AddHours(_fairSettings.DefaultMinimumFairLengthInHours);
        }
        else
        {
            model.Address = fair.Address?.ToModel<AddressModel>();
        }
        model.FairVendorSearchModel.AvailablePageSizes = _fairSettings.PageSizeOptions;
        model.FairVendorSearchModel.SetGridPageSize();

        return Task.FromResult(model);
    }

    public virtual async Task<FairAdminListModel> PrepareFairAdminListModelAsync(FairSearchModel searchModel)
    {
        ThrowIfNull(searchModel);

        var fairs = await _fairService.GetAllFairsAsync(
        name: searchModel.Name,
        isPublished: searchModel.IsPublished,
        isDeleted: searchModel.IsDeleted,
        vendorIds: searchModel.VendorIds,
        fromUtc: searchModel.FromUtc,
        untilUtc: searchModel.UntilUtc,
        pageSize: searchModel.PageSize,
        pageIndex: searchModel.Page - 1);

        var model = new FairAdminListModel().PrepareToGrid(searchModel, fairs, () => fairs.Select(nb => nb.ToModel<FairAdminModel>()));
        return model;
    }

    public async Task<ProductVendorListModel> PrepareFairVendorListModelAsync(FairVendorSearchModel searchModel, Fair fair)
    {
        ThrowIfNull(searchModel, nameof(searchModel));
        ThrowIfNull(fair, nameof(fair));

        var pageIndex = searchModel.Page - 1;
        var pageSize = searchModel.PageSize;

        var maps = await _fairService.GetFairVendorMapsAsync(fair);

        var vendors = await _fairService.GetVendorsByFairIdAsync(fair.Id);
        var objList = await vendors.AsQueryable().ToPagedListAsync(pageIndex, pageSize);
        return new ProductVendorListModel().PrepareToGrid(searchModel, objList, () =>
        {
            var list = new List<FairVendorAdminModel>();
            foreach (var vendor in vendors)
            {
                var map = maps.FirstOrDefault(m => m.VendorId == vendor.Id);
                list.Add(new FairVendorAdminModel
                {
                    Id = map.Id,
                    Name = vendor.Name,
                    DisplayOrder = map.DisplayOrder,
                    AutoApproveProducts = map.AutoApproveProducts,
                });
            }
            ;
            return list;
        });
    }

    public async Task PrepareCreateOrUpdateFairVendorModelAsync(CreateOrUpdateFairVendorModel model)
    {
        model.FairVendorProductSearchModel = new()
        {
            AvailablePageSizes = _fairAdminSettings.PageSizeOptions,
            Length = _fairAdminSettings.DefaultPageSize,
        };

        if (model.VendorId != 0)
        {
            var v = await _vendorService.GetVendorByIdAsync(model.VendorId);
            model.VendorName = v.Name;
            return;
        }
        var fairVendors = await _fairService.GetVendorsByFairIdAsync(model.FairId);

        var vendorList = new List<SelectListItem>();
        await _baseAdminModelFactory.PrepareVendorsAsync(vendorList);

        if (fairVendors?.Any() == true)
        {
            var vendorIds = fairVendors.Select(f => f.Id).Where(x => x != model.VendorId).ToList();
            vendorList = vendorList.Where(d => !vendorIds.Contains(int.Parse(d.Value))).ToList();
        }

        if (model.VendorId != 0)
        {
            vendorList.First(x => model.VendorId == int.Parse(x.Value)).Selected = true;
        }
        if (vendorList.Count > 1)
        {
            vendorList.RemoveAt(0);
            vendorList.First().Selected = true;
        }

        model.AvailableVendors = vendorList;
    }

    public async Task<FairVendorProductListModel> PrepareFairVendorProductListModelAsync(FairVendorProductSearchModel searchModel)
    {
        ThrowIfNull(searchModel.Fair);
        ThrowIfNull(searchModel.Vendor);

        var maps = (await _fairService.GetFairVendorProductMapsAsync(searchModel.Fair, searchModel.Vendor, searchModel.IsApprovedFilter)).ToPagedList(searchModel);
        var model = await new FairVendorProductListModel().PrepareToGridAsync(
            searchModel,
            maps,
            () =>
            {
                return maps.SelectAwait(async m =>
                {
                    var p = await _productService.GetProductByIdAsync(m.ProductId);
                    return new FairVendorProductModel
                    {
                        Id = m.Id,
                        FairId = searchModel.FairId,
                        Name = p.Name,
                        ProductId = p.Id,
                        VendorId = searchModel.VendorId,
                        DisplayOrder = m.FairAdminDisplayOrder,
                        Price = p.Price
                    };
                });
            });
        return model;
    }
    public async Task PrepareAddProductToFairVendorSearchModelAsync(AddProductToFairVendorSearchModel model)
    {
        await _baseAdminModelFactory.PrepareProductTypesAsync(model.AvailableProductTypes ??= []);
        await _baseAdminModelFactory.PrepareCategoriesAsync(model.AvailableCategories ??= []);
        await _baseAdminModelFactory.PrepareManufacturersAsync(model.AvailableManufacturers ??= []);
        model.SetPopupGridPageSize();


    }

    public async Task<FairVendorProductAddPopupListModel> PrepareAddFairVendorProductAddPopupListAsync(AddProductToFairVendorSearchModel searchModel)
    {
        ThrowIfNull(searchModel);

        var products = await _productService.SearchProductsAsync(showHidden: true,
            categoryIds: new List<int> { searchModel.SearchCategoryId },
            manufacturerIds: new List<int> { searchModel.SearchManufacturerId },
            vendorId: searchModel.VendorId,
            //productType: searchModel.SearchProductTypeId > 0 ? (ProductType?)searchModel.SearchProductTypeId : null,
            keywords: searchModel.SearchProductName,
            pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

        var model = new FairVendorProductAddPopupListModel().PrepareToGrid(searchModel, products, () => products.Select(p => p.ToModel<ProductModel>()));

        return model;
    }

}