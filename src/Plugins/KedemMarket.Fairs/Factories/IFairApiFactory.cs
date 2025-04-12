
using KedemMarket.Fairs.Models;

namespace KedemMarket.Fairs.Factories;

public interface IFairApiFactory
{
    Task<IEnumerable<FairApiModel>> PrepareFairListModel(IEnumerable<Fair> fairs);
}
