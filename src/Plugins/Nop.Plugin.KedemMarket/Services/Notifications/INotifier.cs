namespace KedemMarket.Services.Notifications;
public interface INotifier
{
    public Task NotifyVendorsOnNewOrderAsync(IEnumerable<int> vendorIds);
}
