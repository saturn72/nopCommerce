using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;

namespace KedemMarket.Brands.Migrations;

public class BrandBuilder : NopEntityBuilder<Brand>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
        .WithColumn(nameof(Brand.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
        .WithColumn(nameof(Brand.Comment)).AsString(int.MaxValue).Nullable()
        .WithColumn(nameof(Brand.CreatedOnUtc)).AsDateTime2().Nullable().WithDefault(SystemMethods.CurrentUTCDateTime)
        .WithColumn(nameof(Brand.DisplayOrder)).AsInt32().WithDefaultValue(0)
        .WithColumn(nameof(Brand.LogoImageId)).AsInt32().Nullable()
        .WithColumn(nameof(Brand.Name)).AsString(256).NotNullable()
        .WithColumn("SeName").AsString(256).NotNullable();
    }
}
