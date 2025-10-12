using Saturn72.BlogPostExtensions.Domain;
using Nop.Data;

namespace Saturn72.BlogPostExtensions.Services;

public class BlogPostAttributeService : IBlogPostAttributeService
{
    private readonly IRepository<BlogPostAttributes> _blogPostAttributesRepository;

    public BlogPostAttributeService(IRepository<BlogPostAttributes> blogPostAttributesRepository)
    {
        _blogPostAttributesRepository = blogPostAttributesRepository;
    }

    public async Task<BlogPostAttributes?> GetByBlogPostIdAsync(int blogPostId)
    {
        return await (from b in _blogPostAttributesRepository.Table
                      where b.BlogPostId == blogPostId
                      select b).FirstOrDefaultAsync();

    }
}
