using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/04/25 09:52:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250425_0952 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250425_0952(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        if (!Schema.Table(nc.TableNames[typeof(FairVendorProductMap)]).Exists())
            Create.TableFor<FairVendorProductMap>();
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Delete.Table(nc.TableNames[typeof(FairVendorProductMap)]);
    }
}
