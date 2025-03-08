using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/03/06 23:09:08:9037678", "KedemMarket schema", MigrationProcessType.Update)]
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
        Create.TableFor<EventData>();
}
    public override void Down()
    {
        Delete.Table(nameof(EventData));
    }
}
