using FluentMigrator;
using KedemMarket.Domain.Ordering;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/07/16 09:55:08:9037678", "KedemMarket schema", MigrationProcessType.Update)]
public class Migration_20250716_0955 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250716_0955(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        var nc = new KedemMarketNameCompatibility();
        var tn = nc.TableNames[typeof(OrderItemsStatus)];
        if (!Schema.Table(tn).Exists())
            Create.TableFor<OrderItemsStatus>();

    }
    public override void Down()
    {

        if (Schema.Table(nameof(EventData)).Exists())
            Delete.Table(nameof(EventData));
    }
}
