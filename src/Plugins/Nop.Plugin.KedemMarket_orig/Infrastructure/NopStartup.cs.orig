<<<<<<< HEAD
﻿using KedemMarket.Admin.Models.Navbar;
using KedemMarket.Middlewares;
=======
﻿using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using KedemMarket.Admin.Models.Navbar;
using KedemMarket.Middlewares;
using KedemMarket.Services.Agent;
using KedemMarket.Services.Notifications;
using KedemMarket.Services.Vendor;
>>>>>>> dev/get-vendors-sales

namespace KedemMarket.Infrastructure;

public class NopStartup : INopStartup
{
    private const string CorsPolicy = "kedemmarket-api-cors";
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetRequiredSection("cors:origins")
           .AsEnumerable()
           .Where(o => o.Value != default && new Uri(o.Value) != null)
           .Select(o => o.Value)
           .ToArray();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy,
                policy => policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        services.AddMemoryCache();
<<<<<<< HEAD

        services.AddScoped<IExternalUsersService, FirebaseExternalUsersService>();
        services.AddTransient<IValidator<CartTransactionApiModel>, CartTransactionApiModelValidator>();
        services.AddScoped<IKmOrderService, KmOrderService>();

=======
        services.AddSignalR();

        services.TryAddSingleton(new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.All),
        });

        services.AddTransient<IValidator<ChangeVendorOrderStatusRequest>, ChangeVendorOrderStatusRequestValidator>();
        services.AddTransient<IValidator<CartTransactionApiModel>, CartTransactionApiModelValidator>();

        services.AddScoped<IExternalUsersService, FirebaseExternalUsersService>();
        services.AddScoped<IKmOrderService, KmOrderService>();
        services.AddScoped<INotifier, Notifier>();

        services.AddScoped<IAgentSessionService, AgentSessionService>();

        services.AddScoped<IKmVendorService, KmVendorService>();
>>>>>>> dev/get-vendors-sales
        services.AddScoped<IOrderDocumentStore, OrderDocumentStore>();
        services.AddScoped<IUserProfileDocumentStore, UserProfileDocumentStore>();
        services.AddScoped(typeof(IDocumentStore<>), typeof(FirebaseDocumentStore<>));
        services.AddSingleton<FirebaseAdapter>();
        services.AddScoped<IVendorApiModelFactory, VendorApiModelFactory>();
        services.AddScoped<IShoppingCartFactory, ShoppingCartFactory>();
<<<<<<< HEAD
        services.AddScoped<IOrderApiModelFactory, OrderApiModelFactory>();
        services.AddScoped<IDirectoryFactory, DirectoryFactory>();
        services.AddScoped<IHomePageFactory, HomePageFactory>();

=======
        services.AddScoped<Factories.Orders.IOrderModelFactory, Factories.Orders.OrderModelFactory>();
        services.AddScoped<IDirectoryFactory, DirectoryFactory>();
        services.AddScoped<ICmsPagesFactory, CmsPagesFactory>();
>>>>>>> dev/get-vendors-sales
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddTransient<IValidator<EventDataModel>, EventDataModelValidator>();

        //new KM.Common.Infrastructure.NopStartup().ConfigureServices(services, configuration);
        services.AddScoped<INavbarService, NavbarService>();
        services.AddScoped<INavbarFactory, NavbarFactory>();
        services.AddScoped<KedemMarket.Factories.Navbar.INavbarFactory, KedemMarket.Factories.Navbar.NavbarFactory>();
        services.AddScoped<INavbarFactory, NavbarFactory>();
        services.AddTransient<IValidator<NavbarInfoModel>, NavInfoModelValidator>();
        services.AddTransient<IValidator<CreateOrUpdateNavbarElementModel>, CreateNavbarElementPopupModelValidator>();

<<<<<<< HEAD
        services.AddScoped<IEntityToModelFactory, EntityToModelFactory>();

        services.TryAddScoped<IProductApiFactory, ProductApiFactory>();
        services.TryAddSingleton<MediaConvertor>();
=======
        services.AddScoped<ICmsPageModelFactory, CmsPageModelFactory>();

        services.TryAddScoped<IProductApiFactory, ProductApiFactory>();
        services.TryAddScoped<ICategoryApiModelFactory, CategoryApiModelFactory>();
        services.TryAddSingleton<IMediaManager, MediaManager>();
>>>>>>> dev/get-vendors-sales
        services.TryAddSingleton(TimeProvider.System);

        services.TryAddScoped<IStorageManager, GcpStorageManager>();
        //do not configure if already configured
        if (services.FirstOrDefault(x => x.ServiceType == typeof(GcpOptions)) != null)
            return;

        services.Configure<GcpOptions>(options =>
        {
            var bn = configuration["gcpOptions:bucketName"];
            if (string.IsNullOrEmpty(bn) || string.IsNullOrWhiteSpace(bn))
                throw new ArgumentException(nameof(GcpOptions.BucketName));
            options.BucketName = bn;
        });
    }

    public void Configure(IApplicationBuilder application)
    {
        application.UseCors(CorsPolicy);
        application.UseWhen(
<<<<<<< HEAD
            ctx => ctx.Request.Path.StartsWithSegments("/api"),
=======
            ctx => ctx.Request.Path.StartsWithSegments("/api") || ctx.Request.Path.StartsWithSegments("/cms"),
>>>>>>> dev/get-vendors-sales
            appBuilder => appBuilder.UseMiddleware<KedemMarketAuthenticationMiddleware>());
    }

    public int Order => 10;
}