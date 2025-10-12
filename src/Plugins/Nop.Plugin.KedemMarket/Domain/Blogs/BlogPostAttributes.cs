using System;
using Nop.Core;

namespace KedemMarket.Domain.Blogs
{
    public class BlogPostAttributes : BaseEntity
    {
        public int BlogPostId { get; set; }
        public string AuthorIds { get; set; } = string.Empty; // comma-separated
        public string Subtitle { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
    }
}
