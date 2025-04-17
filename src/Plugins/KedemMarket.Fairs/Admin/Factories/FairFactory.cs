namespace KedemMarket.Fairs.Admin.Factories;

public class FairFactory : IFairFactory
{
    private readonly IFairService _fairService;
    private readonly FairSettings _fairSettings;
    private readonly TimeProvider _timeProvider;
    private readonly IBaseAdminModelFactory _baseAdminModelFactory;
    private readonly ILocalizationService _localizationService;
    private readonly IVendorService _vendorService;

    public FairFactory(
        IFairService fairService,
        FairSettings fairSettings,
        TimeProvider timeProvider,
        IBaseAdminModelFactory baseAdminModelFactory,
        ILocalizationService localizationService,
        IVendorService vendorService)
    {
        _fairService = fairService;
        _fairSettings = fairSettings;
        _timeProvider = timeProvider;
        _baseAdminModelFactory = baseAdminModelFactory;
        _localizationService = localizationService;
        _vendorService = vendorService;
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
            model.StartsOnUtc = _timeProvider.GetUtcNow().LocalDateTime;
            model.EndsOnUtc = _timeProvider.GetUtcNow().LocalDateTime.AddHours(_fairSettings.DefaultMinimumFairLengthInHours);
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

        var vendors = await _fairService.GetVendorsByFairIdAsync(fair.Id);
        var objList = await vendors.AsQueryable().ToPagedListAsync(pageIndex, pageSize);
        return new ProductVendorListModel().PrepareToGrid(searchModel, objList, () =>
        {
            var list = new List<FairVendorAdminModel>();
            foreach (var vendor in vendors)
            {
                list.Add(new FairVendorAdminModel
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    DisplayOrder = vendor.DisplayOrder,
                });
            }
            ;
            return list;
        });
    }

    public async Task PrepareCreateOrUpdateFairVendorModelAsync(CreateOrUpdateFairVendorModel model)
    {
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
}