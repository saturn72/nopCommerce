namespace KedemMarket.Factories.Orders;
public interface IOrderModelFactory
{
    public Task<IEnumerable<Nop.Web.Areas.Admin.Models.Orders.OrderModel>> PrepareOrderDetailsModelsAsync(IEnumerable<Order> orders);
    public Task<IEnumerable<VendorOrderModel>> PrepareVendorOrderModelsAsync(IEnumerable<Order> orders, Vendor vendor);
}
