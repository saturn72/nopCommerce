using Nop.Core.Configuration;

namespace Saturn72.BlogPostExtensions
{
    public class BlogPostExtensionsConfiguration : ISettings
    {
        public int MaxSubtitleLength { get; set; } = 256;
        public int MinSubtitleLength { get; set; } = 1;
    }
}
