using FluentValidation;
using KedemMarket.Fair.Admin.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KedemMarket.Fair.Infrastructure;
public class NopStartup : INopStartup
{
    public int Order => 100;

    public void Configure(IApplicationBuilder application)
    {

    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton(TimeProvider.System);
        services.AddScoped<KedemMarket.Fair.Admin.Factories.IFairFactory, KedemMarket.Fair.Admin.Factories.FairFactory>();
        services.AddScoped<IFairService, FairService>();
        services.AddTransient<IValidator<FairInfoAdminModel>, CreateOrUpdateFairInfoValidator>();
        services.AddSingleton<FairCacheSettings>();
    }
}
