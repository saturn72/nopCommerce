namespace KedemMarket.Factories.Orders;
public interface IOrderModelFactory
{
    public Task<IEnumerable<Nop.Web.Areas.Admin.Models.Orders.OrderModel>> PrepareOrderDetailsModelsByVendorIdAsync(IEnumerable<Order> orders, int vendorId);
    public Task<IEnumerable<Nop.Web.Areas.Admin.Models.Orders.OrderModel>> PrepareOrderDetailsModelsAsync(IEnumerable<Order> orders);
}
