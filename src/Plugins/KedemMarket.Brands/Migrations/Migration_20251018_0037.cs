using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Brands.Migrations;

[NopMigration("2025/10/18 00:37:09:9037678", "KedemMarket Brands schema", MigrationProcessType.NoMatter)]

public class Migration_20251018_0037 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly NameCompatibility _nameCompatibility;

    public Migration_20251018_0037(INopDataProvider dataProvider)
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

        Create.Table(tn)
        .WithColumn(nameof(BrandProductMap.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
        .WithColumn(nameof(BrandProductMap.BrandId)).AsInt32().NotNullable()
        .WithColumn(nameof(BrandProductMap.ProductId)).AsInt32().NotNullable().Indexed();
    }

    public override void Down()
    {
        var tn = _nameCompatibility.TableNames[typeof(BrandProductMap)];
        Delete.Table(tn);
    }
}
