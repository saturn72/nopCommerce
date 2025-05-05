using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/05/06 00:21:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250506_0021 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250506_0021(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();
        var table = nc.TableNames[typeof(FairVendorProductMap)];
        Delete.Column("FairAdminDisplayOrder").FromTable(table);
    }

    public override void Down()
    {
    }
}
