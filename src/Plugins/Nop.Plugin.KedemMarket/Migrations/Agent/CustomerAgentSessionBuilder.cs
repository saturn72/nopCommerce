using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using KedemMarket.Domain.Agent;
using Nop.Data.Mapping.Builders;

namespace KedemMarket.Migrations.Agent;
public class CustomerAgentSessionBuilder : NopEntityBuilder<CustomerAgentSession>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CustomerAgentSession.AgentId)).AsString(450).NotNullable()
            .WithColumn(nameof(CustomerAgentSession.CustomerId)).AsInt32().NotNullable()
            .WithColumn(nameof(CustomerAgentSession.CreatedOnUtc)).AsDateTime2().Nullable().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}
