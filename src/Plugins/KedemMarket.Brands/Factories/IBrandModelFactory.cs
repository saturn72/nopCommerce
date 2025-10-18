
namespace KedemMarket.Brands.Factories;
public interface IBrandModelFactory
{
    public Task<BrandSearchModel> PrepareBrandSearchModelAsync(BrandSearchModel searchModel);
    public Task<BrandListModel> PrepareBrandListModelAsync(BrandSearchModel searchModel);
    Task<BrandProductAdminModel> PrepareBrandProductAdminModelAsync(BrandProductAdminModel brandModel, Brand brand);
    Task<IEnumerable<BrandProductAdminModel>> PrepareBrandProductAdminModelsAsync(IList<Brand> brands);
    Task<IEnumerable<BrandProductModel>> PrepareProductBrandProductModelsAsync(int productId);
}
