

namespace KedemMarket.Services.Vendor;
public interface IKmVendorService
{
    public Task<int> GetVendorOpenOrdersCountAsync(int vendorId);
    public Task SetOrdersItemStatusAsync(Order order, List<int> orderItemIds, string status);
}
