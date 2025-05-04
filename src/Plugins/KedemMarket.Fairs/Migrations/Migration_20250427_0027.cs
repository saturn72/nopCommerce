using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/04/27 00:27:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250427_0027 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250427_0027(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(FairVendorProductMap)];
        Alter.Table(table).AddColumn(nameof(FairVendorProductMap.Declined)).AsBoolean().Nullable();
        Alter.Table(table).AddColumn(nameof(FairVendorProductMap.DeclinedOnUtc)).AsDateTime2().Nullable();

        Alter.Column(nameof(FairVendorProductMap.Approved)).OnTable(table).AsBoolean().Nullable();
        Alter.Column(nameof(FairVendorProductMap.ApprovedOnUtc)).OnTable(table).AsDateTime2().Nullable();
    }

    public override void Down()
    {
    }
}
