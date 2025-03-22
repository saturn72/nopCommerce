using Nop.Core.Caching;
using static KedemMarket.Fair.Services.FairConsts;

namespace KedemMarket.Fair.Services;

public class FairService : IFairService
{
    private readonly IRepository<FairInfo> _fairInfoRepository;
    private readonly TimeProvider _timeProvider;
    private readonly IShortTermCacheManager _shortTermCacheManager;
    private readonly FairCacheSettings _fairCacheSettings;

    public FairService(
        IRepository<FairInfo> fairInfoRepository,
        TimeProvider timeProvider,
        IShortTermCacheManager shortTermCacheManager,
        FairCacheSettings fairCacheSettings)
    {
        _fairInfoRepository = fairInfoRepository;
        _timeProvider = timeProvider;
        _shortTermCacheManager = shortTermCacheManager;
        _fairCacheSettings = fairCacheSettings;
    }

    public async Task<IPagedList<FairInfo>> GetAllFairInfosAsync(
        string? name,
        string? datesFilter,
        bool? publishedFilter,
        bool? deletedFilter,
        int pageIndex,
        int pageSize)
    {
        var fairInfos = await _fairInfoRepository.GetAllAsync(async query =>
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

        return new PagedList<FairInfo>(fairInfos, pageIndex, pageSize);
    }

    public Task<FairInfo> GetFairInfoByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(null, nameof(id));

        return _fairInfoRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<FairInfo>> GetFairInfosByNameAsync(string name)
    {
        var cacheKey = _shortTermCacheManager.PrepareKeyForDefaultCache(_fairCacheSettings.GetCacheKeyByFairName(name));
        return await _shortTermCacheManager.GetAsync(() => _fairInfoRepository.Table.Where(c => c.Name == name).ToListAsync(), cacheKey);
    }

    public async Task InsertFairInfoAsync(FairInfo fair)
    {
        await _fairInfoRepository.InsertAsync(fair);
        _shortTermCacheManager.RemoveByPrefix(NopEntityCacheDefaults<FairInfo>.Prefix);
    }

    public async Task UpdateFairInfoAsync(FairInfo fair)
    {
        ThrowIfNull(fair, nameof(fair));
        await _fairInfoRepository.UpdateAsync(fair);
        _shortTermCacheManager.RemoveByPrefix(NopEntityCacheDefaults<FairInfo>.Prefix);
    }

    private IQueryable<FairInfo> FilterByFairDates(IQueryable<FairInfo> query, string fairDateFilter)
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
