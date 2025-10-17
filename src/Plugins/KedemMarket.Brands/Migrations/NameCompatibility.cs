namespace KedemMarket.Brands.Migrations;

public partial class NameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        {typeof(Brand), "km_brands_brand" },
    };

    public Dictionary<(Type, string), string> ColumnName => new();
}