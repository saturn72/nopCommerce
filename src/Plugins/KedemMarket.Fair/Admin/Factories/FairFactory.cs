using KedemMarket.Fair.Admin.Models;

namespace KedemMarket.Fair.Admin.Factories;

public class FairFactory : IFairFactory
{
    private readonly IFairService _fairService;
    private readonly FairSettings _fairSettings;
    private readonly TimeProvider _timeProvider;

    public FairFactory(
        IFairService fairService,
        FairSettings fairSettings,
        TimeProvider timeProvider)
    {
        _fairService = fairService;
        _fairSettings = fairSettings;
        _timeProvider = timeProvider;
    }
    public Task PrepareFairInfoSearchModelAsync(FairInfoSearchModel searchModel)
    {
        searchModel.AvailablePageSizes = _fairSettings.PageSizeOptions;
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
}