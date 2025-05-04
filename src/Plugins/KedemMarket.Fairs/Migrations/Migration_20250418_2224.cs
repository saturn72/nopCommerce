using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/04/18 22:24:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250418_2224 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250418_2224(INopDataProvider dataProvider)
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
        Rename.Column("StartsOnUtc").OnTable(table).To(nameof(Fair.StartsOnLocalDateTime));
        Rename.Column("EndsOnUtc").OnTable(table).To(nameof(Fair.EndsOnLocalDateTime));
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(Fair)];
        Rename.Column(nameof(Fair.StartsOnLocalDateTime)).OnTable(table).To("StartsOnUtc");
        Rename.Column(nameof(Fair.EndsOnLocalDateTime)).OnTable(table).To("EndsOnUtc");
    }
}
