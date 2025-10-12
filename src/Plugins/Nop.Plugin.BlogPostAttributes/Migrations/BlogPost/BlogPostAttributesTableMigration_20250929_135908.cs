using FluentMigrator;
using Nop.Data.Migrations;
using Saturn72.BlogPostExtensions.Domain;

namespace Saturn72.BlogPostExtensions.Migrations.BlogPost;


[NopMigration("2025/09/29 13:59:08:9037678", "BlogPost Attributes schema", MigrationProcessType.NoMatter)]

public class BlogPostAttributesTableMigration_20250929_135908 : Migration
{
    private readonly string _tableName;

    public BlogPostAttributesTableMigration_20250929_135908 ()
    {
        _tableName = new BlogPostExtensionsNameCompatibility().TableNames[typeof(BlogPostAttributes)];
    }
    public override void Up()
    {
        // 1. Alter subtitle column length to 2056
        Alter.Column("Subtitle").OnTable(_tableName).AsString(2056).Nullable();

        // 2. Remove ImageUrl column if exists
        if (Schema.Table(_tableName).Column("ImageUrl").Exists())
            Delete.Column("ImageUrl").FromTable(_tableName);

        // 3. Add PictureId column if not exists
        if (!Schema.Table(_tableName).Column("PictureId").Exists())
            Alter.Table(_tableName).AddColumn("PictureId").AsInt32().Nullable();
    }

    public override void Down()
    {

        // Revert PictureId addition
        if (Schema.Table(_tableName).Column("PictureId").Exists())
            Delete.Column("PictureId").FromTable(_tableName);

        // Revert subtitle length to previous (assume 256)
        Alter.Column("Subtitle").OnTable(_tableName).AsString(256).Nullable();

        // Re-add ImageUrl column
        if (!Schema.Table(_tableName).Column("ImageUrl").Exists())
            Alter.Table(_tableName).AddColumn("ImageUrl").AsString(512).Nullable();
    }
}
