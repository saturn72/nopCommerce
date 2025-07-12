using KedemMarket.Hubs;
using Nop.Web.Framework.Mvc.Routing;

namespace KedemMarket.Infrastructure;
public class RouteProvider : IRouteProvider
{
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapHub<NotificationHub>("/notify");
    }
    public int Priority => 0;
}
