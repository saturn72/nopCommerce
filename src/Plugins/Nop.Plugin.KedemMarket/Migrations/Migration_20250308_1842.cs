using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/03/08 18:42:08:9037678", "KedemMarket schema", MigrationProcessType.Update)]
public class Migration_20250306_2212 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250306_2212(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        var nc = new KedemMarketNameCompatibility();
        if (!Schema.Table(nc.TableNames[typeof(EventData)]).Exists())
            Create.TableFor<EventData>();
    }
    public override void Down()
    {

        if (Schema.Table(nameof(EventData)).Exists())
            Delete.Table(nameof(EventData));
    }
}
