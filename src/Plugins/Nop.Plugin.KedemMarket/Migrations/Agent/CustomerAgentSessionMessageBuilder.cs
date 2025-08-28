using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using KedemMarket.Domain.Agent;
using Nop.Data.Mapping.Builders;

namespace KedemMarket.Migrations.Agent;

public class CustomerAgentSessionMessageBuilder : NopEntityBuilder<CustomerAgentSessionMessage>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CustomerAgentSessionMessage.MessageSource)).AsString(450).NotNullable()
            .WithColumn(nameof(CustomerAgentSessionMessage.Content)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(CustomerAgentSessionMessage.CustomerAgentSessionId)).AsInt32().NotNullable()
            .WithColumn(nameof(CustomerAgentSessionMessage.UtcTimestamp)).AsDateTime2().Nullable().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}