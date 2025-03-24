
namespace KedemMarket.Fair.Admin.Factories;

public class FairFactory : IFairFactory
{
    private readonly IFairService _fairService;
    private readonly FairSettings _fairSettings;
    private readonly TimeProvider _timeProvider;
    private readonly IVendorService _vendorService;

    public FairFactory(
        IFairService fairService,
        FairSettings fairSettings,
        TimeProvider timeProvider,
        IVendorService vendorService)
    {
        _fairService = fairService;
        _fairSettings = fairSettings;
        _timeProvider = timeProvider;
        _vendorService = vendorService;
    }
    public Task PrepareFairInfoSearchModelAsync(FairInfoSearchModel searchModel)
    {
        //searchModel.PAgeS= _fairSettings.PageSizeOptions;
        searchModel.SetGridPageSize();

        return Task.CompletedTask;
    }

    public Task<FairInfoAdminModel> PrepareFairInfoAdminModelAsync(FairInfoAdminModel model, FairInfo fairInfo)
    {
        if (fairInfo != null)
            //fill in model values from the entity
            if (model == null)
                model = fairInfo.ToModel<FairInfoAdminModel>();

        model ??= new FairInfoAdminModel();
        //set default values for the new model
        if (fairInfo == null)
        {
            model.Published = false;
            model.Deleted = false;
            model.StartsOnUtc = _timeProvider.GetUtcNow().LocalDateTime;
            model.EndsOnUtc = _timeProvider.GetUtcNow().LocalDateTime.AddHours(_fairSettings.DefaultFairLengthInHours);

            //TODO: set default values for the new model
            // vendor products - can publish freely
        }
        model.FairVendorSearchModel.AvailablePageSizes = _fairSettings.PageSizeOptions;
        model.FairVendorSearchModel.SetGridPageSize();

        return Task.FromResult(model);
    }

    public virtual async Task<FairInfoAdminListModel> PrepareFairInfoListModelAsync(FairInfoSearchModel searchModel)
    {
        ThrowIfNull(searchModel);

        var fairs = await _fairService.GetAllFairInfosAsync(
        name: searchModel.Name,
        datesFilter: searchModel.DateFilter,
        publishedFilter: searchModel.PublishedFilter,
        deletedFilter: searchModel.DeletedFilter,
        pageIndex: searchModel.Page - 1,
        pageSize: searchModel.PageSize);

        var model = new FairInfoAdminListModel().PrepareToGrid(searchModel, fairs, () => fairs.Select(nb => nb.ToModel<FairInfoAdminModel>()));
        return model;
    }

    public async Task<ProductVendorListModel> PrepareFairInfoVendorListModelAsync(FairVendorSearchModel searchModel, FairInfo fair)
    {
        ThrowIfNull(searchModel, nameof(searchModel));
        ThrowIfNull(fair, nameof(fair));

        var fairVendorMaps = await _fairService.GetFairVendorsByFairInfoIdAsync(
            fairId: fair.Id,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var allVendors = fairVendorMaps.Any() ? (await _vendorService.GetAllVendorsAsync()).ToList() : new List<Vendor>();

        return new ProductVendorListModel().PrepareToGrid(searchModel, fairVendorMaps, () =>
        {
            var list = new List<FairVendorModel>();
            foreach (var fvm in fairVendorMaps)
            {
                var vendor = allVendors.FirstOrDefault(x => x.Id == fvm.VendorId);
                list.Add(new FairVendorModel
                {
                    Id = fvm.Id,
                    Name = vendor.Name,
                    DisplayOrder = fvm.DisplayIndex,
                });
            };
            return list;
        });
        //searchModel.Vendors = fairVendorMaps.Select(fvm =>
        //{
        //    var vendor = allVendors.FirstOrDefault(x => x.Id == fvm.VendorId);
        //    return new FairVendorModel
        //    {
        //        Id = fvm.Id,
        //        Name = vendor.Name,
        //        DisplayOrder = fvm.DisplayIndex,
        //    };
        //}).ToList();
    }
}