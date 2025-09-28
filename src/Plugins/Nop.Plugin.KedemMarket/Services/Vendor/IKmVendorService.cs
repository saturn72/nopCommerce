

using KedemMarket.Domain.Ordering;

namespace KedemMarket.Services.Vendor;
public interface IKmVendorService
{
    public Task<int> GetVendorOpenOrdersCountAsync(int vendorId);
    public Task SetOrdersItemStatusAsync(Order order, List<int> orderItemIds, string status);
    public Task<OrderItemsStatus> GetOrdersItemsStatusAsync(int orderId);
}
