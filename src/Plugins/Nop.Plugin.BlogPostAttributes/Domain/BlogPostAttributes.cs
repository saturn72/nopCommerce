using Nop.Core;

namespace Saturn72.BlogPostExtensions.Domain;

public class BlogPostAttributes : BaseEntity
{
    public int BlogPostId { get; set; }
    public string? AuthorIds { get; set; }
    public string? Subtitle { get; set; }
    public int PictureId { get; set; }
}
