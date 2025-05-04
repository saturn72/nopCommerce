using Nop.Data.Mapping;

namespace KedemMarket.Fairs.Migrations;

public partial class KedemMarketFairNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        {typeof(Fair), "km_fair" },
        {typeof(FairAddressMap), "km_fairaddressmap" },
        {typeof(FairCustomerFavoriteMap), "km_fair_customer_favorite_map" },
        {typeof(FairVendorMap), "km_fairvendormap" },
        {typeof(FairVendorProductMap), "km_fair_vendor_product_map" },
    };

    public Dictionary<(Type, string), string> ColumnName => new();
}