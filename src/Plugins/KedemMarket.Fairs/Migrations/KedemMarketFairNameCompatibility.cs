using Nop.Data.Mapping;

namespace KedemMarket.Fairs.Migrations;

public partial class KedemMarketFairNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        {typeof(Fair), "km_fair" },
        {typeof(FairVendorMap), "km_fairvendormap" },
        {typeof(FairAddressMap), "km_fairaddressmap" },
    };

    public Dictionary<(Type, string), string> ColumnName => new();
}