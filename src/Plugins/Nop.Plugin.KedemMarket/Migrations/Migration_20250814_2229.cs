using FluentMigrator;
using KedemMarket.Domain.Agent;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/08/14 22:08:29:9037678", "KedemMarket schema", MigrationProcessType.NoMatter)]
public class Migration_20250814_2229 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly KedemMarketNameCompatibility _nameCompatibility;

    public Migration_20250814_2229(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _nameCompatibility = new KedemMarketNameCompatibility();
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        CreateTableIfNotExists<CustomerAgentSession>();
    }

    private void CreateTableIfNotExists<TEntity>() where TEntity : BaseEntity
    {
        if (!Schema.Table(_nameCompatibility.TableNames[typeof(TEntity)]).Exists())
            Create.TableFor<TEntity>();

    }
    public override void Down()
    {
        var table = _nameCompatibility.TableNames[typeof(CustomerAgentSession)];
        if (!Schema.Table(table).Exists())
            Delete.Table(table);
    }
}
