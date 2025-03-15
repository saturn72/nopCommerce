using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/03/08 18:42:08:9037678", "KedemMarket schema", MigrationProcessType.Update)]
public class Migration_20250308_1842 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    KedemMarketNameCompatibility _nc = new();

    public Migration_20250308_1842(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (Schema.Table("eventdata").Exists())
            Delete.Table("eventdata");

        if (!Schema.Table(_nc.TableNames[typeof(EventData)]).Exists())
            Create.TableFor<EventData>();
    }
    public override void Down()
    {
        if (Schema.Table(_nc.TableNames[typeof(EventData)]).Exists())
            Delete.Table(_nc.TableNames[typeof(EventData)]);
        //Create.TableFor<EventData>();
    }
}
