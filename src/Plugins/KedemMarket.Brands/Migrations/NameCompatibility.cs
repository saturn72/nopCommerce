namespace KedemMarket.Brands.Migrations;

public partial class NameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        {typeof(Brand), "km_brands_brand" },
        {typeof(BrandProductMap), "km_brands_brand_product_map" },
    };

    public Dictionary<(Type, string), string> ColumnName => new();
}