namespace KedemMarket.Fair.Services;
public interface IFairService
{
    Task<IPagedList<FairInfo>> GetAllFairInfosAsync(string? name, string? datesFilter, bool? publishedFilter, bool? deletedFilter, int pageIndex, int pageSize);
    Task<FairInfo> GetFairInfoByIdAsync(int id);
    Task<IEnumerable<FairInfo>> GetFairInfosByNameAsync(string name);
    Task<IPagedList<FairVendorMap>> GetFairVendorsByFairInfoIdAsync(int fairId, int pageIndex = 0, int pageSize = int.MaxValue);
    Task InsertFairInfoAsync(FairInfo fair);
    Task UpdateFairInfoAsync(FairInfo fair);
}

public class FairConsts
{
    public class CustomerRoleNames
    {
        public const string FairManager = "Fair Manager";
    }

    public class FairDateFilter
    {
        public const string All = "all";
        public const string ShowActiveOnly = "active";
        public const string ShowEndedOnly = "ended";
        public const string ShowFutureOnly = "future";
    }
}
