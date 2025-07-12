using KedemMarket.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace KedemMarket.Services.Notifications;

public class Notifier : INotifier
{
    private readonly IHubContext<NotificationHub> _notifucationHub;

    public Notifier(IHubContext<NotificationHub> notifucationHub)
    {
        _notifucationHub = notifucationHub;
    }

    public async Task NotifyVendorsOnNewOrderAsync(IEnumerable<int> vendorIds)
    {
        if (vendorIds == null || !vendorIds.Any())
            return;

        await _notifucationHub.Clients.All.SendAsync("IncomingMessage", new
        {
            key = "order-new",
            payload = new { vendorIds = vendorIds.ToArray() },
        });
    }
}
