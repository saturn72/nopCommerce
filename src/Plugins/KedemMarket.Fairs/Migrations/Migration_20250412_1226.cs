using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/04/12 12:26:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250412_1226 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250412_1226(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(Fair)];

        Alter.Column(nameof(Fair.StartsOnLocalDateTime))
            .OnTable(table)
            .AsDateTime()
            .Nullable();

        Alter.Column(nameof(Fair.EndsOnLocalDateTime))
            .OnTable(table)
            .AsDateTime()
            .Nullable();
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(Fair)];

        Alter.Column(nameof(Fair.StartsOnLocalDateTime))
            .OnTable(table)
            .AsDateTime();

        Alter.Column(nameof(Fair.EndsOnLocalDateTime))
            .OnTable(table)
            .AsDateTime();
    }
}
