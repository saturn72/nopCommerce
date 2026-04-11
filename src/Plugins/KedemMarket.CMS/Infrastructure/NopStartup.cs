using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using KedemMarket.Cms.Factories.Pages;
using KedemMarket.CMS.Services;

namespace KedemMarket.Cms.Infrastructure;

public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetRequiredSection("cors:origins")
           .AsEnumerable()
           .Where(o => o.Value != default && new Uri(o.Value) != null)
           .Select(o => o.Value)
           .ToArray();

        services.AddMemoryCache();
        services.AddSignalR();

        services.TryAddSingleton(new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.All),
        });

        services.AddScoped<ICmsPagesFactory, CmsPagesFactory>();
        services.AddScoped<ICmsPageModelFactory, CmsPageModelFactory>();
        services.AddSingleton<ICacheKeyProvider,CacheKeyProvider>();
    }

    public void Configure(IApplicationBuilder application)
    {
        //application.UseWhen(
        //   ctx => ctx.Request.Path.StartsWithSegments("/api/cms") || ctx.Request.Path.StartsWithSegments("/cms"),
        //   appBuilder => appBuilder.UseMiddleware<KedemMarketAuthenticationMiddleware>());
    }

    public int Order => 10;
}