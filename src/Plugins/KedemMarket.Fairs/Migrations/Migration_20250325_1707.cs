using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/03/25 17:07:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250325_1707 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250325_1707(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();

        Alter.Table(nc.TableNames[typeof(Fair)]).AddColumn(nameof(Fair.CustomerId)).AsInt32().NotNullable();
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Delete.Column(nameof(Fair.CustomerId)).FromTable(nc.TableNames[typeof(Fair)]);
    }
}
