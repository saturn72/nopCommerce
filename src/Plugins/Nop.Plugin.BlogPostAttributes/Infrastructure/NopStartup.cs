using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Saturn72.BlogPostExtensions.Services;

namespace Saturn72.BlogPostExtensions.Infrastructure;

public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBlogPostAttributeService, BlogPostAttributeService>();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 100;
}
