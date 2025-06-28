using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/03/06 22:12:08:9037678", "KedemMarket schema", MigrationProcessType.Update)]
public class Migration_20250308_1842 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250308_1842(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketNameCompatibility nc = new();
        var tableName = nc.TableNames[typeof(EventData)];
        if (Schema.Table(tableName).Exists())
            Delete.Table(tableName);

        if (!Schema.Table(tableName).Exists())
            Create.TableFor<EventData>();
    }
    public override void Down()
    {
        KedemMarketNameCompatibility nc = new();
        var tableName = nc.TableNames[typeof(EventData)];

        if (Schema.Table(tableName).Exists())
            Delete.Table(tableName);
    }
}
