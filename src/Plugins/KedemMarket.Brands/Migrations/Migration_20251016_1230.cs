using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Brands.Migrations;

[NopMigration("2025/10/16 12:30:09:9037678", "KedemMarket Brands schema", MigrationProcessType.NoMatter)]

public class Migration_20251016_1230 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly NameCompatibility _nameCompatibility;

    public Migration_20251016_1230(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _nameCompatibility = new NameCompatibility();
    }

    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        Alter.Table(_nameCompatibility.TableNames[typeof(Brand)])
            .AddColumn(nameof(Brand.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
    public override void Down()
    {
        Delete.Column(nameof(Brand.UpdatedOnUtc)).FromTable(_nameCompatibility.TableNames[typeof(Brand)]);
    }
}
