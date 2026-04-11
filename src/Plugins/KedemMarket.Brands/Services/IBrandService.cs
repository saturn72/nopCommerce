namespace KedemMarket.Brands.Services;
public interface IBrandService
{
    public Task<PagedList<Brand>> GetAllBrandsAsync(int pageIndex = 0, int pageSize = int.MaxValue, string[]? names = null);
    public Task<Brand> GetBrandByIdAsync(int brandId);
    public Task<IList<Brand>> GetProductBrandsByProductIdAsync(int productId);
}
