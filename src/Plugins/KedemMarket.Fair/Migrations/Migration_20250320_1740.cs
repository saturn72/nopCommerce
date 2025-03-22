using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fair.Migrations;

[NopMigration("2025/03/20 17:40:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250320_1740 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250320_1740(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        var t = typeof(FairInfo);
        Alter.Table(nc.TableNames[t])
               .AddColumn(nameof(FairInfo.CreatedOnUtc)).AsDateTime2().WithDefaultValue(SystemMethods.CurrentUTCDateTime);

        Alter.Table(nc.TableNames[t])
               .AddColumn(nameof(FairInfo.UpdatedOnUtc)).AsDateTime2().Nullable();

        Alter.Table(nc.TableNames[t])
               .AddColumn(nameof(FairInfo.DeletedOnUtc)).AsDateTime2().Nullable();
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();
        var tableName = nc.TableNames[typeof(FairInfo)];
        Delete.Column(nameof(FairInfo.DeletedOnUtc)).FromTable(tableName);
        Delete.Column(nameof(FairInfo.UpdatedOnUtc)).FromTable(tableName);
        Delete.Column(nameof(FairInfo.CreatedOnUtc)).FromTable(tableName);
    }
}
