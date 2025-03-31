using Nop.Core.Caching;
using static KedemMarket.Fairs.Services.FairConsts;

namespace KedemMarket.Fairs.Services;

public class FairService : IFairService
{
    private readonly IRepository<Fair> _fairRepository;
    private readonly IRepository<FairAddressMap> _fairAddressMapRepository;
    private readonly IRepository<FairVendorMap> _fairVendorMapRepository;
    private readonly IRepository<Address> _addressRepository;
    private readonly TimeProvider _timeProvider;
    private readonly IShortTermCacheManager _shortTermCacheManager;
    private readonly FairCacheSettings _fairCacheSettings;

    public FairService(
        IRepository<Fair> fairRepository,
        IRepository<FairAddressMap> fairAddressMapRepository,
        IRepository<FairVendorMap> fairVendorMapRepository,
        TimeProvider timeProvider,
        IShortTermCacheManager shortTermCacheManager,
        FairCacheSettings fairCacheSettings,
        IRepository<Address> addressRepository)
    {
        _fairRepository = fairRepository;
        _fairAddressMapRepository = fairAddressMapRepository;
        _fairVendorMapRepository = fairVendorMapRepository;
        _timeProvider = timeProvider;
        _shortTermCacheManager = shortTermCacheManager;
        _fairCacheSettings = fairCacheSettings;
        _addressRepository = addressRepository;
    }

    public async Task DeleteFairVendorMapAsync(FairVendorMap fairVendorMap)
    {
        ThrowIfNull(fairVendorMap, nameof(fairVendorMap));
        await _fairVendorMapRepository.DeleteAsync(fairVendorMap);
    }

    public async Task<IPagedList<Fair>> GetAllFairsAsync(
        string? name,
        string? datesFilter,
        bool? publishedFilter,
        bool? deletedFilter,
        int pageIndex,
        int pageSize)
    {
        var fairs = await _fairRepository.GetAllAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));

            if (datesFilter != null || datesFilter != FairDateFilter.All)
                query = FilterByFairDates(query, datesFilter);

            if (publishedFilter.HasValue)
                query = query.Where(c => c.Published == publishedFilter.Value);

            if (deletedFilter.HasValue)
                query = query.Where(c => c.Deleted == deletedFilter.Value);

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
        var cacheKey = _shortTermCacheManager.PrepareKeyForDefaultCache(_fairCacheSettings.GetCacheKeyByFairName(name, customerId));
        return await _shortTermCacheManager.GetAsync(() => _fairRepository.Table.Where(c => c.CustomerId == customerId && c.Name == name).ToListAsync(), cacheKey);
    }

    public async Task<FairVendorMap> GetFairVendorMapByIdAsync(int fairVendorMapId)
    {
        return await _fairVendorMapRepository.GetByIdAsync(fairVendorMapId);
    }

    public async Task<IEnumerable<FairVendorMap>> GetFairVendorMapsByFairIdAsync(int fairId, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        if (fairId <= 0)
            return [];

        var query = _fairVendorMapRepository.Table.Where(fvm => fvm.FairId == fairId);
        return await query.ToListAsync();
    }

    public async Task InsertFairAsync(Fair fair)
    {
        await _fairRepository.InsertAsync(fair);
        _shortTermCacheManager.RemoveByPrefix(NopEntityCacheDefaults<Fair>.Prefix);
    }
    public async Task InsertFairVendorMapAsync(FairVendorMap fairVendorMap)
    {
        await _fairVendorMapRepository.InsertAsync(fairVendorMap);
        _shortTermCacheManager.RemoveByPrefix(NopEntityCacheDefaults<FairVendorMap>.Prefix);
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
        _shortTermCacheManager.RemoveByPrefix(NopEntityCacheDefaults<Fair>.Prefix);
    }

    public async Task UpdateFairVendorMapAsync(FairVendorMap fairVendorMap)
    {
        ThrowIfNull(fairVendorMap, nameof(fairVendorMap));
        await _fairVendorMapRepository.UpdateAsync(fairVendorMap);
        _shortTermCacheManager.RemoveByPrefix(NopEntityCacheDefaults<Fair>.Prefix);
    }

    private IQueryable<Fair> FilterByFairDates(IQueryable<Fair> query, string fairDateFilter)
    {
        var curDate = _timeProvider.GetUtcNow().DateTime;
        return fairDateFilter switch
        {
            FairDateFilter.ShowActiveOnly => query.Where(f => f.EndsOnUtc >= curDate),
            FairDateFilter.ShowEndedOnly => query.Where(f => f.EndsOnUtc < curDate),
            FairDateFilter.ShowFutureOnly => query.Where(f => f.StartsOnUtc > curDate),
            _ => query,
        };
    }
}
