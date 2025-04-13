
using KedemMarket.Fairs.Models;

namespace KedemMarket.Fairs.Factories;

public interface IFairApiFactory
{
    Task<FairListApiModel> PrepareFairApiModelListAsync(IEnumerable<Fair> fairs);
    Task<FairApiModel> PrepareFairApiModelAsync(Fair fair);
}
