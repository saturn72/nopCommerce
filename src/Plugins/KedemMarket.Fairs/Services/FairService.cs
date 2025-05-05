using Nop.Core.Caching;
using Nop.Core.Events;

namespace KedemMarket.Fairs.Services;

public class FairService : IFairService
{
    private readonly IRepository<Fair> _fairRepository;
    private readonly IRepository<FairAddressMap> _fairAddressMapRepository;
    private readonly IRepository<FairVendorMap> _fairVendorMapRepository;
    private readonly IRepository<Vendor> _vendorRepository;
    private readonly IRepository<Address> _addressRepository;
    private readonly IRepository<FairCustomerFavoriteMap> _fairCustomerFavoriteMapRepository;
    private readonly IRepository<FairVendorProductMap> _fairVendorProductMapRepository;

    private readonly TimeProvider _timeProvider;
    private readonly IShortTermCacheManager _shortTermCacheManager;
    private readonly FairCacheSettings _fairCacheSettings;
    private readonly IEventPublisher _eventPublisher;

    public FairService(
        IRepository<Fair> fairRepository,
        IRepository<FairAddressMap> fairAddressMapRepository,
        IRepository<FairVendorMap> fairVendorMapRepository,
        TimeProvider timeProvider,
        IShortTermCacheManager shortTermCacheManager,
        FairCacheSettings fairCacheSettings,
        IRepository<Address> addressRepository,
        IEventPublisher eventPublisher,
        IRepository<FairCustomerFavoriteMap> fairCustomerFavoriteMapRepository,
        IRepository<Vendor> vendorRepository,
        IRepository<FairVendorProductMap> fairVendorProductMapRepository)
    {
        _fairRepository = fairRepository;
        _fairAddressMapRepository = fairAddressMapRepository;
        _fairVendorMapRepository = fairVendorMapRepository;
        _timeProvider = timeProvider;
        _shortTermCacheManager = shortTermCacheManager;
        _fairCacheSettings = fairCacheSettings;
        _addressRepository = addressRepository;
        _eventPublisher = eventPublisher;
        _fairCustomerFavoriteMapRepository = fairCustomerFavoriteMapRepository;
        _vendorRepository = vendorRepository;
        _fairVendorProductMapRepository = fairVendorProductMapRepository;
    }

    public async Task DeleteFairAsync(Fair fair)
    {
        ThrowIfNull(fair, nameof(fair));
        fair.Deleted = true;
        fair.Published = false;
        fair.DeletedOnUtc = _timeProvider.GetUtcNow().UtcDateTime;

        await _fairRepository.UpdateAsync(fair, false);
        await _eventPublisher.EntityDeletedAsync(fair);
    }

    public async Task DeleteFairVendorMapAsync(FairVendorMap fairVendorMap)
    {
        ThrowIfNull(fairVendorMap, nameof(fairVendorMap));
        await _fairVendorMapRepository.DeleteAsync(fairVendorMap);
    }

    public async Task<IPagedList<Fair>> GetAllFairsAsync(
        string? name = null,
        bool? isPublished = true,
        bool? isDeleted = null,
        IEnumerable<int> vendorIds = null,
        DateTime? fromUtc = null,
        DateTime? untilUtc = null,
        int pageSize = int.MaxValue,
        int pageIndex = 0)
    {
        var fairs = await _fairRepository.GetAllAsync(async query =>
        {
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));

            if (isPublished.HasValue)
                query = query.Where(f => f.Published == isPublished);

            if (isDeleted.HasValue)
                query = query.Where(f => f.Deleted == isDeleted);

            fromUtc ??= _timeProvider.GetUtcNow().UtcDateTime;
            query = query.Where(f => fromUtc <= f.StartsOnLocalDateTime);

            if (untilUtc.HasValue)
                query = query.Where(f => untilUtc <= f.EndsOnLocalDateTime);

            if (vendorIds?.Any() == true)
            {
                var fairIds = await _fairVendorMapRepository.Table
                    .Where(m => vendorIds.Contains(m.VendorId))
                    .Select(m => m.FairId)
                    .Distinct()
                    .ToListAsync();

                query = query.Where(c => fairIds.Contains(c.Id));
            }

            return query;
        });

        return new PagedList<Fair>(fairs, pageIndex, pageSize);
    }

    public async Task<Fair> GetFairByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(null, nameof(id));

        var fair = await _fairRepository.GetByIdAsync(id);
        var map = await _fairAddressMapRepository.Table.FirstOrDefaultAsync(x => x.FairId == fair.Id);

        if (map != null)
            fair.Address = await _addressRepository.GetByIdAsync(map.AddressId);

        return fair;
    }
    public async Task<IEnumerable<Fair>> GetFairsByNameAsync(string name, int customerId)
    {
        var cacheKey = _shortTermCacheManager.PrepareKeyForDefaultCache(_fairCacheSettings.GetFairCacheKeyByFairName(name, customerId));
        return await _shortTermCacheManager.GetAsync(() => _fairRepository.Table.Where(c => c.CustomerId == customerId && c.Name == name).ToListAsync(), cacheKey);
    }

    public async Task<FairVendorMap> GetFairVendorMapByIdAsync(int id)
    {
        return await _fairVendorMapRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<FairVendorMap>> GetFairVendorMapsAsync(Fair fair)
    {
        ThrowIfNull(fair);

        var ck = _fairCacheSettings.GetFairVendorMapsCacheKeyByFairId(fair.Id);
        var cacheKey = _shortTermCacheManager.PrepareKeyForDefaultCache(ck);

        return await _shortTermCacheManager.GetAsync(
            () => _fairVendorMapRepository.Table.Where(m => m.FairId == fair.Id).ToListAsync(),
            cacheKey);
    }

    public async Task<IEnumerable<Vendor>> GetVendorsByFairIdAsync(int fairId)
    {
        var maps = await _fairVendorMapRepository.Table
            .Where(x => x.FairId == fairId)
            .ToListAsync();
        var vendorIds = maps.Select(x => x.VendorId).ToList();
        return await _vendorRepository.GetByIdsAsync(vendorIds);
    }

    public async Task InsertFairAsync(Fair fair)
    {
        await _fairRepository.InsertAsync(fair);
    }
    public async Task InsertFairVendorMapAsync(FairVendorMap fairVendorMap)
    {
        await _fairVendorMapRepository.InsertAsync(fairVendorMap);
    }

    public async Task SetFairFavoriteAsync(Customer customer, Fair fair, bool value)
    {
        ThrowIfNull(customer, nameof(customer));
        ThrowIfNull(fair, nameof(fair));

        var f = await _fairCustomerFavoriteMapRepository.Table.FirstOrDefaultAsync(c => c.CustomerId == customer.Id && c.FairId == fair.Id);

        if (!value)
        {
            if (f != null)
                await _fairCustomerFavoriteMapRepository.DeleteAsync(f);
        }
        else
        {
            if (f == null)
            {
                f = new FairCustomerFavoriteMap
                {
                    CustomerId = customer.Id,
                    FairId = fair.Id,
                };
                await _fairCustomerFavoriteMapRepository.InsertAsync(f);
            }
        }
    }

    public async Task UpdateFairAsync(Fair fair)
    {
        ThrowIfNull(fair, nameof(fair));
        await _fairRepository.UpdateAsync(fair);
        var fam = await _fairAddressMapRepository.Table.FirstOrDefaultAsync(x => x.FairId == fair.Id);

        if (fam == null)
        {
            if (!fair.IsVirtual && fair.Address != null)
            {
                await _addressRepository.InsertAsync(fair.Address);
                fam = new()
                {
                    AddressId = fair.Address.Id,
                    FairId = fair.Id,
                };
                await _fairAddressMapRepository.InsertAsync(fam);
            }
        }
        else
        {
            if (fair.IsVirtual || fair.Address == null)
            {
                //remove exist address
                await _addressRepository.DeleteAsync(d => d.Id == fam.AddressId);
                await _fairAddressMapRepository.DeleteAsync(fam);
            }
            else
            {
                var a = await _addressRepository.GetByIdAsync(fam.AddressId);
                if (a.City != fair.Address.City ||
                a.Address1 != fair.Address.Address1 ||
                a.Address2 != fair.Address.Address2)
                {
                    a.City = fair.Address.City;
                    a.Address1 = fair.Address.Address1;
                    a.Address2 = fair.Address.Address2;
                    await _addressRepository.UpdateAsync(a);
                }
            }
        }
    }

    public async Task UpdateFairVendorMapAsync(FairVendorMap fairVendorMap)
    {
        ThrowIfNull(fairVendorMap, nameof(fairVendorMap));
        await _fairVendorMapRepository.UpdateAsync(fairVendorMap);
    }

    public Task<FairVendorProductMap> GetFairVendorProductMapByIdAsync(int id)
    {
        return _fairVendorProductMapRepository.GetByIdAsync(id);
    }
    public async Task<IList<FairVendorProductMap>> GetFairVendorProductMapsAsync(
        Fair fair,
        Vendor vendor,
        bool? isApprovedFilter = null,
        int pageSize = int.MaxValue,
        int pageIndex = 0)
    {
        //at this point we ignore pagination
        var p = _fairCacheSettings.GetFairVendorProductMapCacheKey(fair.Name, vendor.Id, pageIndex, pageSize);
        var cacheKey = _shortTermCacheManager.PrepareKeyForDefaultCache(p);

        return await _shortTermCacheManager.GetAsync(() => _fairVendorProductMapRepository.Table
                    .Where(m => m.FairId == fair.Id && m.VendorId == vendor.Id)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync(), cacheKey);
    }

    public async Task InsertFairVendorProductMapsAsync(IList<FairVendorProductMap> maps)
    {
        await _fairVendorProductMapRepository.InsertAsync(maps);
    }
    public async Task UpdateFairVendorProductMapsAsync(IList<FairVendorProductMap> maps)
    {
        await _fairVendorProductMapRepository.UpdateAsync(maps);
    }

    public async Task DeleteFairVendorProductMapsAsync(FairVendorProductMap map)
    {
        await _fairVendorProductMapRepository.DeleteAsync(map);
    }
}
