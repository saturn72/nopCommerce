namespace KedemMarket.Fair.Admin.Factories;
public interface IFairFactory
{
    Task PrepareFairInfoSearchModelAsync(FairInfoSearchModel searchModel);
    Task<FairInfoAdminModel> PrepareFairInfoAdminModelAsync(FairInfoAdminModel model, FairInfo fairInfo);
    Task<FairInfoAdminListModel> PrepareFairInfoListModelAsync(FairInfoSearchModel searchModel);
    Task<ProductVendorListModel> PrepareFairInfoVendorListModelAsync(FairVendorSearchModel searchModel, FairInfo fair);
}
