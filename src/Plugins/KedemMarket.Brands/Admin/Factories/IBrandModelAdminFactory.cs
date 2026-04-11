
namespace KedemMarket.Brands.Admin.Factories;
public interface IBrandModelAdminFactory
{
    public Task<BrandSearchModel> PrepareBrandSearchModelAsync(BrandSearchModel searchModel);
    public Task<BrandListModel> PrepareBrandListModelAsync(BrandSearchModel searchModel);
    Task<BrandProductAdminModel> PrepareBrandProductAdminModelAsync(BrandProductAdminModel brandModel, Brand brand);
    Task<IList<BrandProductAdminModel>> PrepareBrandProductAdminModelsAsync(IList<Brand> brands);
    
}
