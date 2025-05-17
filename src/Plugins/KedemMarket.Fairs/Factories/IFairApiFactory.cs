using KedemMarket.Fairs.Models;

namespace KedemMarket.Fairs.Factories;

public interface IFairApiFactory
{
    Task<FairListApiModel> PrepareFairApiSlimModelListAsync(IEnumerable<Fair> fairs);
    Task<FairApiModel> PrepareFairApiModelAsync(Fair fair);
    Task<FairVendorMapApiModel> PrepareFairVendorMapApiModelAsync(
        FairVendorMap map,
        bool includeFairInfo,
        bool includeVendorProducts,
        bool? showApprovedProductOnly = true);
}
