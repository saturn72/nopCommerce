using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/04/17 22:28:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250417_2228 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250417_2228(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        Alter.Table(nc.TableNames[typeof(Fair)])
            .AddColumn(nameof(Fair.PictureId)).AsInt32().WithDefaultValue(0).Nullable();
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Delete.Column(nameof(Fair.PictureId)).FromTable(nc.TableNames[typeof(Fair)]);
    }
}
