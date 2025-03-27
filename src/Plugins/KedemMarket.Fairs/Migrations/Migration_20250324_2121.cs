using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Fairs.Migrations;

[NopMigration("2025/03/24 21:21:08:9037678", "KedemMarket Fair schema", MigrationProcessType.NoMatter)]

public class Migration_20250324_2121 : Migration
{
    protected readonly INopDataProvider _dataProvider;

    public Migration_20250324_2121(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        KedemMarketFairNameCompatibility nc = new();

        Rename.Table("km_fairinfo").To(nc.TableNames[typeof(Fair)]);
        Rename.Column("FairInfoId").OnTable(nc.TableNames[typeof(FairVendorMap)]).To(nameof(FairVendorMap.FairId));
    }
    public override void Down()
    {
        KedemMarketFairNameCompatibility nc = new();

        Rename.Table(nameof(Fair)).To("km_fairinfo");
        Rename.Column(nameof(FairVendorMap.FairId)).OnTable(nc.TableNames[typeof(FairVendorMap)]).To("FairInfoId");
    }
}
