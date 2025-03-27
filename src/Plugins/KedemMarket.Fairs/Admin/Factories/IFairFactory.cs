namespace KedemMarket.Fairs.Admin.Factories;
public interface IFairFactory
{
    Task PrepareFairSearchModelAsync(FairSearchModel searchModel);
    Task<FairAdminModel> PrepareFairAdminModelAsync(FairAdminModel model, Fair fair);
    Task<FairAdminListModel> PrepareFairAdminListModelAsync(FairSearchModel searchModel);
    Task<ProductVendorListModel> PrepareFairVendorListModelAsync(FairVendorSearchModel searchModel, Fair fair);
    Task PrepareCreateOrUpdateFairVendorModelAsync(CreateOrUpdateFairVendorModel model);
}
