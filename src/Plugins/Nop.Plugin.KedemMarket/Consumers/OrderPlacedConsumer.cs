using KedemMarket.Services.Vendor;

namespace KedemMarket.Consumers;
public class OrderPlacedConsumer : IConsumer<EntityInsertedEvent<Order>>
{
    private readonly IOrderService _orderService;
    private readonly IKmVendorService _kmVendorService;

    public OrderPlacedConsumer(
        IOrderService orderService,
        IKmVendorService kmVendorService)
    {
        _orderService = orderService;
        _kmVendorService = kmVendorService;
    }
    public async Task HandleEventAsync(EntityInsertedEvent<Order> eventMessage)
    {
        var order = eventMessage.Entity;
        if (order == null)
            return;

        var orderItems = await _orderService.GetOrderItemsAsync(order.Id);
        if (orderItems == null || orderItems.Count == 0)
            return;

        var orderItemIds = orderItems.Select(d => d.Id).ToList();
        if (orderItemIds.Count == 0)
            return;

        await _kmVendorService.SetOrdersItemStatusAsync(order, orderItemIds, KmConsts.OrderStatuses.Pending);
    }
}
