using Nop.Core.Caching;
using static KedemMarket.Fairs.Services.FairConsts;

namespace KedemMarket.Fairs.Services;

public class FairService : IFairService
{
    private readonly IRepository<Fair> _fairRepository;
    private readonly IRepository<FairVendorMap> _fairVendorMapRepository;
    private readonly TimeProvider _timeProvider;
    private readonly IShortTermCacheManager _shortTermCacheManager;
    private readonly FairCacheSettings _fairCacheSettings;

    public FairService(
        IRepository<Fair> fairRepository,
        IRepository<FairVendorMap> fairVendorMapRepository,
        TimeProvider timeProvider,
        IShortTermCacheManager shortTermCacheManager,
        FairCacheSettings fairCacheSettings)
    {
        _fairRepository = fairRepository;
        _timeProvider = timeProvider;
        _shortTermCacheManager = shortTermCacheManager;
        _fairCacheSettings = fairCacheSettings;
        _fairVendorMapRepository = fairVendorMapRepository;
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

    public Task<Fair> GetFairByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(null, nameof(id));

        return _fairRepository.GetByIdAsync(id);
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
