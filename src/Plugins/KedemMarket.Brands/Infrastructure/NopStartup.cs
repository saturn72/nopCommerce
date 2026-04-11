using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KedemMarket.Brands.Infrastructure;
public class NopStartup : INopStartup
{
    public int Order => 100;
    internal const string HTTP_CLIENT_NAME = "brands-http-client";

    public void Configure(IApplicationBuilder application)
    {

    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IBrandModelAdminFactory, BrandModelAdminFactory>();
        services.AddScoped<IBrandModelFactory, BrandModelFactory>();
        services.AddScoped<IBrandImportExportManager, BrandImportExportManager>();

        services.AddHttpClient(HTTP_CLIENT_NAME)
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                MaxConnectionsPerServer = 100 // Default is often too low
            });
    }
}
