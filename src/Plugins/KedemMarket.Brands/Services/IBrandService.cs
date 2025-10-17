namespace KedemMarket.Brands.Services;
public interface IBrandService
{
    public Task<PagedList<Brand>> GetAllBrandsAsync(int pageIndex = 0, int pageSize = int.MaxValue, string[]? names = null);
}
