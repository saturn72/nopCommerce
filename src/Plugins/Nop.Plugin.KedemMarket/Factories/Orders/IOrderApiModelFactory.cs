namespace KedemMarket.Factories.Orders;
public interface IOrderApiModelFactory
{
    public Task<IEnumerable<Nop.Web.Areas.Admin.Models.Orders.OrderModel>> PrepareOrderDetailsModelsAsync(IEnumerable<Order> orders);
}
