namespace KedemMarket.Brands.Services;

public class BrandService : IBrandService
{
    private readonly IRepository<Brand> _brandRepository;

    public BrandService(IRepository<Brand> brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<PagedList<Brand>> GetAllBrandsAsync(int pageIndex = 0, int pageSize = int.MaxValue, string[]? names = null)
    {
        if (pageSize == int.MaxValue)
            pageSize = int.MaxValue - 1;

        var brandsQuery = _brandRepository.Table;

        var tNames = names?.Where(n => !string.IsNullOrEmpty(n) && !string.IsNullOrWhiteSpace(n)).Select(n => n.Trim()).ToArray();
        if (tNames != null && tNames.Any())
            brandsQuery = brandsQuery.Where(b => tNames.Any(n => b.Name.Contains(n, StringComparison.OrdinalIgnoreCase)));
        var brands = await brandsQuery
            .OrderBy(b => b.DisplayOrder)
            .ThenBy(b => b.Name)
            .ToListAsync();

        return new PagedList<Brand>(brands, pageIndex, pageSize);
    }
}