using FluentMigrator;
using Nop.Data.Migrations;
using Saturn72.BlogPostExtensions.Domain;

namespace Saturn72.BlogPostExtensions.Migrations.BlogPost;

[NopMigration("2025/09/29 11:29:06:9037678", "BlogPost Attributes schema", MigrationProcessType.Installation)]
public class BlogPostAttributesTableUpdateMigration_20250929_112906 : Migration
{
    private readonly string _tableName;

    public BlogPostAttributesTableUpdateMigration_20250929_112906()
    {
        _tableName = new BlogPostExtensionsNameCompatibility().TableNames[typeof(BlogPostAttributes)];
    }

    public override void Up()
    {

        if (!Schema.Table(_tableName).Exists())
        {
            Create.Table(_tableName)
            .WithColumn(nameof(BlogPostAttributes.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(BlogPostAttributes.BlogPostId)).AsInt32().NotNullable()
            .WithColumn(nameof(BlogPostAttributes.AuthorIds)).AsString(512).Nullable()
            .WithColumn(nameof(BlogPostAttributes.Subtitle)).AsString(512).Nullable()
            .WithColumn(nameof(BlogPostAttributes.PictureId)).AsString(1024).Nullable();
        }
    }

    public override void Down()
    {
        if (Schema.Table(_tableName).Exists())
            Delete.Table(_tableName);
    }
}
