using Nop.Data.Mapping;

namespace KedemMarket.Fairs.Migrations;

public partial class KedemMarketFairNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        {typeof(Domain.Fair), "km_fair" },
        {typeof(FairVendorMap), "km_fairvendormap" },
    };

    public Dictionary<(Type, string), string> ColumnName => new();
}