
using Nop.Web.Areas.Admin.Models.Catalog;

namespace KedemMarket.Fairs.Admin.Factories;
public interface IFairFactory
{
    public Task PrepareFairSearchModelAsync(FairSearchModel searchModel);
    public Task<FairAdminModel> PrepareFairAdminModelAsync(FairAdminModel model, Fair fair);
    public Task<FairAdminListModel> PrepareFairAdminListModelAsync(FairSearchModel searchModel);
    public Task<ProductVendorListModel> PrepareFairVendorListModelAsync(FairVendorSearchModel searchModel, Fair fair);
    public Task PrepareCreateOrUpdateFairVendorModelAsync(CreateOrUpdateFairVendorModel model);
    public Task<FairVendorProductListModel> PrepareFairVendorProductListModelAsync(FairVendorProductSearchModel searchModel);
    public Task PrepareAddProductToFairVendorSearchModelAsync(AddProductToFairVendorSearchModel model);
    Task<FairVendorProductAddPopupListModel> PrepareAddFairVendorProductAddPopupListAsync(AddProductToFairVendorSearchModel searchModel);
}
