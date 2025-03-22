using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fair.Migrations;

[NopMigration("2025/03/19 22:33:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250319_2233 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly KedemMarketFairNameCompatibility _nameCompatibility;

    public Migration_20250319_2233(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _nameCompatibility = new KedemMarketFairNameCompatibility();
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        CreateTableIfNotExists<FairInfo>();
    }

    private void CreateTableIfNotExists<TEntity>() where TEntity : BaseEntity
    {
        if (!Schema.Table(_nameCompatibility.TableNames[typeof(TEntity)]).Exists())
            Create.TableFor<TEntity>();

    }
    public override void Down()
    {
        Delete.Table(_nameCompatibility.TableNames[typeof(FairInfo)]);
    }
}
