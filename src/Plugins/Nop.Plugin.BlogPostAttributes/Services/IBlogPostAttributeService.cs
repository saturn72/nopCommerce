using Saturn72.BlogPostExtensions.Domain;

namespace Saturn72.BlogPostExtensions.Services;
public interface IBlogPostAttributeService
{
    public Task<BlogPostAttributes?> GetByBlogPostIdAsync(int blogPostId);
}
