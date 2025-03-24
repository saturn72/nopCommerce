using Nop.Data.Mapping;

namespace KedemMarket.Fair.Migrations;

public partial class KedemMarketFairNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        {typeof(FairInfo), "km_fairinfo" },
        {typeof(FairVendorMap), "km_fairvendormap" },
    };

    public Dictionary<(Type, string), string> ColumnName => new();
}