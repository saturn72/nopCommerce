namespace KedemMarket.Brands.Factories;
public interface IBrandModelFactory
{
    public Task<BrandDetailsModel> PrepareBrandDetailsModelAsync(Brand brand);
    public Task<IList<BrandProductModel>> PrepareProductBrandProductModelsAsync(int productId);
}
