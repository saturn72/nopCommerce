using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Brands.Migrations;

[NopMigration("2025/10/18 03:25:09:9037678", "KedemMarket Brands schema", MigrationProcessType.NoMatter)]

public class Migration_20251018_0325 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly NameCompatibility _nameCompatibility;

    public Migration_20251018_0325(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _nameCompatibility = new NameCompatibility();
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        var tn = _nameCompatibility.TableNames[typeof(BrandProductMap)];
        Alter
            .Column(nameof(Brand.CreatedOnUtc))
            .OnTable(tn)
            .AsDateTime()
            .NotNullable()
            .WithDefault(SystemMethods.CurrentUTCDateTime);
    }

    public override void Down()
    {
        var tn = _nameCompatibility.TableNames[typeof(BrandProductMap)];
        Alter
            .Column(nameof(Brand.CreatedOnUtc))
            .OnTable(tn)
            .AsDateTime()
            .Nullable()
            .WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}
