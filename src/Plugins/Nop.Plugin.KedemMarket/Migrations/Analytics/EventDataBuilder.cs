using FluentMigrator.Builders.Create.Table;
using FluentMigrator;
using Nop.Data.Mapping.Builders;

namespace KedemMarket.Migrations.Analytics;
public class EventDataBuilder : NopEntityBuilder<EventData>
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