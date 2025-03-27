using FluentValidation;
using KedemMarket.Fairs.Admin.Models;
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
        services.AddScoped<KedemMarket.Fairs.Admin.Factories.IFairFactory, KedemMarket.Fairs.Admin.Factories.FairFactory>();
        services.AddScoped<IFairService, FairService>();
        services.AddTransient<IValidator<FairAdminModel>, CreateOrUpdateFairValidator>();
        services.AddSingleton<FairCacheSettings>();
    }
}
