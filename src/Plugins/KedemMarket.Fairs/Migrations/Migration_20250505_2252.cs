using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/05/05 22:52:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250505_2252 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250505_2252(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(FairVendorProductMap)];
        Alter.Table(table).AddColumn(nameof(FairVendorProductMap.ProductName)).AsString(400).NotNullable();
        Rename.Column("FairVendorDisplayOrder").OnTable(table).To(nameof(FairVendorProductMap.DisplayOrder));
    }

    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(FairVendorProductMap)];
        Delete.Column(nameof(FairVendorProductMap.ProductName)).FromTable(table);
        Rename.Column(nameof(FairVendorProductMap.DisplayOrder)).OnTable(table).To("FairVendorDisplayOrder");
    }
}
