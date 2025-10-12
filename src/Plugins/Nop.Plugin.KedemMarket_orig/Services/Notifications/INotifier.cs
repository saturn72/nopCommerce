
namespace KedemMarket.Services.Notifications;
public interface INotifier
{
    public Task NotifyCustomerOnOrderStausChangedAsync(Order order);
    public Task OnNewOrderAsync(string userId, IEnumerable<int> vendorIds, int orderId);
}
