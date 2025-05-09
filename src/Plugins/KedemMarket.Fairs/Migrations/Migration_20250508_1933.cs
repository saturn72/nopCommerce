using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/05/08 19:33:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250508_1933 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250508_1933(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(FairVendorProductMap)];
        Alter.Table(table).AlterColumn(nameof(FairVendorProductMap.Approved)).AsBoolean().NotNullable().WithDefaultValue(false);
        Alter.Table(table).AddColumn(nameof(FairVendorProductMap.PendingApproval)).AsBoolean().NotNullable().WithDefaultValue(true);
    }

    public override void Down()
    {
    }
}
