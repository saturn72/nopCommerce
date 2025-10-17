using FluentMigrator;
using Nop.Data;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace KedemMarket.Brands.Migrations;

[NopMigration("2025/10/15 21:16:29:9037678", "KedemMarket Brands schema", MigrationProcessType.NoMatter)]

public class Migration_20251015_2115 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly NameCompatibility _nameCompatibility;

    public Migration_20251015_2115(INopDataProvider dataProvider)
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
        if (!Schema.Table(tn).Exists())
            Create.TableFor<Brand>();
    }
    public override void Down()
    {
    }
}
