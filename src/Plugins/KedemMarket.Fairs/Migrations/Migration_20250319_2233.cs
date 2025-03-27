using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/03/19 22:33:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250320_1740 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250320_1740(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        if (!Schema.Table(nc.TableNames[typeof(Domain.Fair)]).Exists())
            Create.TableFor<Domain.Fair>();

    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        var tableName = nc.TableNames[typeof(Domain.Fair)];
        Delete.Table(tableName);
    }
}
