using DocumentFormat.OpenXml.Drawing;
using FluentMigrator;
using KedemMarket.Domain.Agent;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/08/14 23:03:29:9037678", "KedemMarket schema", MigrationProcessType.NoMatter)]
public class Migration_20250814_2303 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly KedemMarketNameCompatibility _nameCompatibility;

    public Migration_20250814_2303(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _nameCompatibility = new KedemMarketNameCompatibility();
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        var tn = _nameCompatibility.TableNames[typeof(CustomerAgentSession)];
        if (Schema.Table(tn).Exists())
            Delete.Table(tn);

        Create.TableFor<CustomerAgentSession>();

        var casTable = _nameCompatibility.TableNames[typeof(CustomerAgentSession)];
        Create.Index("IX_UserAgentSession_CustomerId_AgentId")
        .OnTable(casTable)
        .OnColumn(nameof(CustomerAgentSession.CustomerId)).Ascending()
        .OnColumn(nameof(CustomerAgentSession.AgentId)).Ascending();

        Create.TableFor<CustomerAgentSessionMessage>();
        var casmTable = _nameCompatibility.TableNames[typeof(CustomerAgentSessionMessage)];
        Create.Index("IX_UserAgentSessionMessage_CustomerAgentSessionId")
       .OnTable(casmTable)
       .OnColumn(nameof(CustomerAgentSessionMessage.CustomerAgentSessionId)).Ascending();
    }
    public override void Down()
    {
    }
}
