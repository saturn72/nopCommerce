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

        if (names != null && names.Any())
            brandsQuery = brandsQuery.Where(b => names.Contains(b.Name, StringComparer.OrdinalIgnoreCase));
        var brands = await brandsQuery.OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();


        return new PagedList<Brand>(brands, pageIndex, pageSize);
    }
}