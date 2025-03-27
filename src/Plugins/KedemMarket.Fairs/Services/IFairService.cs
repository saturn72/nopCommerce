namespace KedemMarket.Fairs.Services;
public interface IFairService
{
    Task<IPagedList<Fair>> GetAllFairsAsync(string? name, string? datesFilter, bool? publishedFilter, bool? deletedFilter, int pageIndex, int pageSize);
    Task<Fair> GetFairByIdAsync(int id);
    Task<IEnumerable<Fair>> GetFairsByNameAsync(string name, int customerId);
    Task<IEnumerable<FairVendorMap>> GetFairVendorMapsByFairIdAsync(int fairId, int pageIndex = 0, int pageSize = int.MaxValue);
    Task<FairVendorMap> GetFairVendorMapByIdAsync(int fairVendorMapId);
    Task InsertFairAsync(Fair fair);
    Task InsertFairVendorMapAsync(FairVendorMap fairVendorMap);
    Task UpdateFairAsync(Fair fair);
    Task UpdateFairVendorMapAsync(FairVendorMap fairVendorMap);
    Task DeleteFairVendorMapAsync(FairVendorMap fairVendorMap);
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
