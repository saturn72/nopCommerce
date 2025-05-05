using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/05/05 03:26:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250505_0326 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250505_0326(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(FairVendorProductMap)];
        Alter.Table(table).AddColumn(nameof(FairVendorProductMap.ProductPrice)).AsDecimal(18,2).Nullable();
    }

    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(FairVendorProductMap)];
        Delete.Column(nameof(FairVendorProductMap.ProductPrice)).FromTable(table);
    }
}
