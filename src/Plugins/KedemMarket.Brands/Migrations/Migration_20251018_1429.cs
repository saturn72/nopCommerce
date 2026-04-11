using FluentMigrator;
using Nop.Data.Migrations;

namespace KedemMarket.Brands.Migrations;

[NopMigration("2025/10/18 14:29:09:9037678", "Add columns to `Brand` table", MigrationProcessType.NoMatter)]

public class Migration_20251018_1429 : Migration
{
    protected readonly INopDataProvider _dataProvider;
    private readonly NameCompatibility _nameCompatibility;

    public Migration_20251018_1429(INopDataProvider dataProvider)
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
        Alter
            .Table(tn)
            .AddColumn(nameof(Brand.Description)).AsString(int.MaxValue).Nullable()
            .AddColumn(nameof(Brand.MetaDescription)).AsString(int.MaxValue).Nullable()
            .AddColumn(nameof(Brand.MetaKeywords)).AsString(int.MaxValue).Nullable()
            .AddColumn(nameof(Brand.MetaTitle)).AsString(int.MaxValue).Nullable();

        var tn1 = _nameCompatibility.TableNames[typeof(BrandProductMap)];
        Delete.Column(nameof(Brand.MetaTitle)).FromTable(tn1);
        Delete.Column(nameof(Brand.MetaKeywords)).FromTable(tn1);
        Delete.Column(nameof(Brand.MetaDescription)).FromTable(tn1);
        Delete.Column(nameof(Brand.Description)).FromTable(tn1);
    }

    public override void Down()
    {
        var tn = _nameCompatibility.TableNames[typeof(Brand)];
        Delete.Column(nameof(Brand.MetaTitle)).FromTable(tn);
        Delete.Column(nameof(Brand.MetaKeywords)).FromTable(tn);
        Delete.Column(nameof(Brand.MetaDescription)).FromTable(tn);
        Delete.Column(nameof(Brand.Description)).FromTable(tn);
    }
}
