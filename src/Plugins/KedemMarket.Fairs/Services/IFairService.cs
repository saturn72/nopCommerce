namespace KedemMarket.Fairs.Services;
public interface IFairService
{
    Task<IPagedList<Fair>> GetAllFairsAsync(
        string? name = null,
        bool? isPublished = true,
        bool? isDeleted = null,
        DateTime? fromUtc = null,
        DateTime? untilUtc = null,
        int pageSize = int.MaxValue,
        int pageIndex = 0);
    Task<Fair> GetFairByIdAsync(int id);
    Task<IEnumerable<Fair>> GetFairsByNameAsync(string name, int customerId);
    Task<IEnumerable<FairVendorMap>> GetFairVendorMapsByFairIdAsync(int fairId, int pageIndex = 0, int pageSize = int.MaxValue);
    Task<FairVendorMap> GetFairVendorMapByIdAsync(int fairVendorMapId);
    Task InsertFairAsync(Fair fair);
    Task InsertFairVendorMapAsync(FairVendorMap fairVendorMap);
    Task UpdateFairAsync(Fair fair);
    Task DeleteFairAsync(Fair fair);
    Task UpdateFairVendorMapAsync(FairVendorMap fairVendorMap);
    Task DeleteFairVendorMapAsync(FairVendorMap fairVendorMap);
}
