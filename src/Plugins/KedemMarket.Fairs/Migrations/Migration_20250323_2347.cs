using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/03/23 23:47:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250323_2347 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250323_2347(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        if (!Schema.Table(nc.TableNames[typeof(FairVendorMap)]).Exists())
            Create.TableFor<FairVendorMap>();

    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Delete.Table(nc.TableNames[typeof(FairVendorMap)]);
    }
}
