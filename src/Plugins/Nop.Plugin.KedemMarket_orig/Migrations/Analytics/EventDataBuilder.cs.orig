using FluentMigrator.Builders.Create.Table;
using FluentMigrator;
using Nop.Data.Mapping.Builders;

namespace KedemMarket.Migrations.Analytics;
<<<<<<< HEAD
public class EventDataBuilder : NopEntityBuilder<EventData>
=======
public class CustomerAgentSessionBuilder : NopEntityBuilder<EventData>
>>>>>>> dev/get-vendors-sales
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(EventData.Data)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(EventData.EventName)).AsString(256).NotNullable()
            .WithColumn(nameof(EventData.PerformedByKMUserId)).AsString(256).Nullable()
            .WithColumn(nameof(EventData.PerformedByNopUserId)).AsInt32().NotNullable()
            .WithColumn(nameof(EventData.PerformedOnUtc)).AsDateTime2().Nullable().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}