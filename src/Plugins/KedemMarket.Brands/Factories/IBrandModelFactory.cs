namespace KedemMarket.Brands.Factories;
public interface IBrandModelFactory
{
    public Task<BrandSearchModel> PrepareBrandSearchModelAsync(BrandSearchModel searchModel);
    public Task<BrandListModel> PrepareBrandListModelAsync(BrandSearchModel searchModel);
    Task<BrandModel> PrepareBrandModelAsync(BrandModel brandModel, Brand brand);
}
