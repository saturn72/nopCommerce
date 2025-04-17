using FluentValidation;
using KedemMarket.Common.Infrastructure;
using KedemMarket.Fairs.Factories;
using KedemMarket.Fairs.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KedemMarket.Fairs.Infrastructure;
public class NopStartup : INopStartup
{
    public int Order => 100;

    public void Configure(IApplicationBuilder application)
    {

    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton(TimeProvider.System);
        services.AddScoped<IFairFactory, FairFactory>();
        services.AddScoped<IFairService, FairService>();
        services.AddSingleton<FairCacheSettings>();
        services.AddTransient<IFairApiFactory, FairApiFactory>();
        new CommonServiceConfigurar().ConfigureServices(services, configuration);

        services.AddTransient<IValidator<FairAdminModel>, CreateOrUpdateFairValidator>();
        services.AddTransient<IValidator<SetFairFavoriteRequest>, SetFairFavoriteRequestValidator>();
    }
}
