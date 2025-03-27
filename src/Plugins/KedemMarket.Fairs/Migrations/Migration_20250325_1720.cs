using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/03/25 17:20:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250325_1720 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250325_1720(INopDataProvider dataProvider)
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
            .AddColumn(nameof(Fair.IsVirtual)).AsBoolean().NotNullable();

        Alter.Table(nc.TableNames[typeof(Fair)])
            .AlterColumn(nameof(Fair.Deleted)).AsBoolean().Nullable()
            .AlterColumn(nameof(Fair.Description)).AsString(int.MaxValue).Nullable()
            .AlterColumn(nameof(Fair.Name)).AsString(400).NotNullable()
            .AlterColumn(nameof(Fair.Published)).AsBoolean().Nullable()
            .AlterColumn(nameof(Fair.StartsOnUtc)).AsDateTime2().Nullable()
            .AlterColumn(nameof(Fair.EndsOnUtc)).AsDateTime2().Nullable()
            .AlterColumn(nameof(Fair.CreatedOnUtc)).AsDateTime2().NotNullable()
            .AlterColumn(nameof(Fair.DeletedOnUtc)).AsDateTime2().Nullable()
            .AlterColumn(nameof(Fair.UpdatedOnUtc)).AsDateTime2().Nullable()
            .AlterColumn(nameof(Fair.CustomerId)).AsInt32().NotNullable();


    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        Delete.Column(nameof(Fair.IsVirtual)).FromTable(nc.TableNames[typeof(Fair)]);
    }
}
