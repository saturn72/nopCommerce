using Saturn72.BlogPostExtensions.Domain;
using Nop.Data.Mapping;

namespace Saturn72.BlogPostExtensions.Migrations;

/// <summary>
/// Backward compatibility of table naming for BlogPostAttributes plugin
/// </summary>
public partial class BlogPostExtensionsNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        { typeof(BlogPostAttributes), "saturn72_blogpostattributes" }
    };

    public Dictionary<(Type, string), string> ColumnName => new();
}
