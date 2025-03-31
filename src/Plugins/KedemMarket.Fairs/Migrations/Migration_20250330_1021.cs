using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/03/30 10:21:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250330_1021 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250330_1021(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        if (!Schema.Table(nc.TableNames[typeof(FairAddressMap)]).Exists())
            Create.TableFor<FairAddressMap>();
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Delete.Table(nc.TableNames[typeof(FairAddressMap)]);
    }
}
