using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/04/25 23:59:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250425_2359 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250425_2359(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        Alter.Table(nc.TableNames[typeof(FairVendorMap)]).AddColumn(nameof(FairVendorMap.AutoApproveProducts)).AsBoolean().WithDefaultValue(false);
    }

    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Delete.Column(nameof(FairVendorMap.AutoApproveProducts)).FromTable(nc.TableNames[typeof(FairVendorMap)]);
    }
}
