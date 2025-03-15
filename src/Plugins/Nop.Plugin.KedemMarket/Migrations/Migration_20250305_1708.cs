using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Migrations;

[NopMigration("2025/03/05 17:08:08:9037678", "KedemMarket schema", MigrationProcessType.Update)]
public class Migration_20250305_1708 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250305_1708(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        var nc = new KedemMarketNameCompatibility();
        var nevType = typeof(NavbarElementVendor);
        Alter.Table(nc.TableNames[nevType])
               .AddColumn(nameof(NavbarElementVendor.PublishWhatsapp)).AsBoolean().WithDefaultValue(false);
    }
    public override void Down()
    {
        var nc = new KedemMarketNameCompatibility();
        var nevType = typeof(NavbarElementVendor);
        Delete.Column(nameof(NavbarElementVendor.PublishWhatsapp)).FromTable(nc.TableNames[nevType]);
    }
}
