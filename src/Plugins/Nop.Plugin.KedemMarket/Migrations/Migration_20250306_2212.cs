using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/03/08 18:42:08:9037678", "KedemMarket schema", MigrationProcessType.Update)]
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
        Delete.Table("eventdata");
        Create.TableFor<EventData>();
}
    public override void Down()
    {
        var nc = new KedemMarketNameCompatibility();
        Delete.Table(nc.TableNames[typeof(EventData)]);
        //Create.TableFor<EventData>();
    }
}
