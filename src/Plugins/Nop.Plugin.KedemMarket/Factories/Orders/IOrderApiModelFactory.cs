namespace KedemMarket.Factories.Orders;
public interface IOrderApiModelFactory
{
    Task<IEnumerable<OrderInfoModel>> PrepareOrderDetailsModelsAsync(IEnumerable<Order> orders);
}
