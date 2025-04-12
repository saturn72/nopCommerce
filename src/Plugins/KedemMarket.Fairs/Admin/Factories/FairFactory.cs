namespace KedemMarket.Fairs.Admin.Factories;

public class FairFactory : IFairFactory
{
    private readonly IFairService _fairService;
    private readonly FairSettings _fairSettings;
    private readonly TimeProvider _timeProvider;
    private readonly IVendorService _vendorService;
    private readonly IBaseAdminModelFactory _baseAdminModelFactory;
    private readonly ILocalizationService _localizationService;

    public FairFactory(
        IFairService fairService,
        FairSettings fairSettings,
        TimeProvider timeProvider,
        IVendorService vendorService,
        IBaseAdminModelFactory baseAdminModelFactory,
        ILocalizationService localizationService)
    {
        _fairService = fairService;
        _fairSettings = fairSettings;
        _timeProvider = timeProvider;
        _vendorService = vendorService;
        _baseAdminModelFactory = baseAdminModelFactory;
        _localizationService = localizationService;
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
            model.EndsOnUtc = _timeProvider.GetUtcNow().LocalDateTime.AddHours(_fairSettings.DefaultFairLengthInHours);
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
        isDeleted: searchModel.IsDeleted,
        startsOnUtc: searchModel.StartsOnUtc,
        endsOnUtc: searchModel.StartsOnUtc,
        pageSize: searchModel.PageSize,
        skip: int.MaxValue);

        var model = new FairAdminListModel().PrepareToGrid(searchModel, fairs, () => fairs.Select(nb => nb.ToModel<FairAdminModel>()));
        return model;
    }

    public async Task<ProductVendorListModel> PrepareFairVendorListModelAsync(FairVendorSearchModel searchModel, Fair fair)
    {
        ThrowIfNull(searchModel, nameof(searchModel));
        ThrowIfNull(fair, nameof(fair));

        var pageIndex = searchModel.Page - 1;
        var pageSize = searchModel.PageSize;

        var fvms = await _fairService.GetFairVendorMapsByFairIdAsync(
            fairId: fair.Id,
            pageIndex,
            pageSize);

        var allVendors = fvms.Any() ? (await _vendorService.GetAllVendorsAsync()).ToList() : new List<Vendor>();

        var objList = await fvms.AsQueryable().ToPagedListAsync(pageIndex, pageSize);
        return new ProductVendorListModel().PrepareToGrid(searchModel, objList, () =>
        {
            var list = new List<FairVendorModel>();
            foreach (var fvm in fvms)
            {
                var vendor = allVendors.FirstOrDefault(x => x.Id == fvm.VendorId);
                list.Add(new FairVendorModel
                {
                    Id = fvm.Id,
                    Name = vendor.Name,
                    DisplayOrder = fvm.DisplayOrder,
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
        var fvms = await _fairService.GetFairVendorMapsByFairIdAsync(model.FairId);

        var av = new List<SelectListItem>();
        await _baseAdminModelFactory.PrepareVendorsAsync(av);

        if (fvms?.Any() == true)
        {
            var vendorIds = fvms.Select(f => f.VendorId).Where(x => x != model.VendorId).ToList();
            av = av.Where(d => !vendorIds.Contains(int.Parse(d.Value))).ToList();
        }

        if (model.VendorId != 0)
        {
            av.First(x => model.VendorId == int.Parse(x.Value)).Selected = true;
        }
        if (av.Count > 1)
        {
            av.RemoveAt(0);
            av.First().Selected = true;
        }

        model.AvailableVendors = av;
    }
}