using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/03/25 22:05:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250325_2205 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250325_2205(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        Rename.Column("DisplayIndex").OnTable(nc.TableNames[typeof(FairVendorMap)]).To(nameof(FairVendorMap.DisplayOrder));
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Rename.Column(nameof(FairVendorMap.DisplayOrder)).OnTable(nc.TableNames[typeof(FairVendorMap)]).To("DisplayIndex");
    }
}
