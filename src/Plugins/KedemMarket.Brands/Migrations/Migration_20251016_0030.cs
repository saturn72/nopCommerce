using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Brands.Migrations;

[NopMigration("2025/10/16 00:30:53:9037678", "KedemMarket Brands schema", MigrationProcessType.NoMatter)]

public class Migration_20251016_0030 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly NameCompatibility _nameCompatibility;

    public Migration_20251016_0030(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _nameCompatibility = new NameCompatibility();
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        var tn = _nameCompatibility.TableNames[typeof(Brand)];
        Delete.Column("SeName").FromTable(tn);
    }
    public override void Down()
    {
        Alter.Table(_nameCompatibility.TableNames[typeof(Brand)])
            .AddColumn("SeName").AsString(256).NotNullable();
    }
}
