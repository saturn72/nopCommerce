
namespace KedemMarket.Fairs.Services;
public interface IFairService
{
    Task<IPagedList<Fair>> GetAllFairsAsync(
        string? name = null,
        bool? isPublished = true,
        bool? isDeleted = null,
        IEnumerable<int> vendorIds = null,
        DateTime? fromUtc = null,
        DateTime? untilUtc = null,
        int pageSize = int.MaxValue,
        int pageIndex = 0);
    Task<Fair> GetFairByIdAsync(int id);
    Task<IEnumerable<Fair>> GetFairsByNameAsync(string name, int customerId);
    Task<IEnumerable<Vendor>> GetVendorsByFairIdAsync(int fairId);
    Task<FairVendorMap> GetFairVendorMapByIdAsync(int id);
    Task<IEnumerable<FairVendorMap>> GetFairVendorMapsAsync(Fair fair);
    public Task InsertFairAsync(Fair fair);
    public Task InsertFairVendorMapAsync(FairVendorMap fairVendorMap);
    public Task UpdateFairAsync(Fair fair);
    public Task DeleteFairAsync(Fair fair);
    public Task UpdateFairVendorMapAsync(FairVendorMap fairVendorMap);
    public Task DeleteFairVendorMapAsync(FairVendorMap fairVendorMap);
    public Task SetFairFavoriteAsync(Customer customer, Fair fair, bool value);
    Task<IList<FairVendorProductMap>> GetFairVendorProductMapsAsync(
        Fair fair,
        Vendor vendor,
        bool? isApprovedFilter = null,
        int pageSize = int.MaxValue,
        int pageIndex = 0);
    public Task InserFairVendorProductMapAsync(FairVendorProductMap map);
}
