using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/04/13 20:57:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250413_2057 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250413_2057(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        if (!Schema.Table(nc.TableNames[typeof(FairCustomerFavoriteMap)]).Exists())
            Create.TableFor<FairCustomerFavoriteMap>();
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Delete.Table(nc.TableNames[typeof(FairCustomerFavoriteMap)]);
    }
}
