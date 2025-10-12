
using KedemMarket.Domain.Ordering;
using KedemMarket.Services.Notifications;

namespace KedemMarket.Consumers;
public class OrderItemsStatusChangedConsumer :
    IConsumer<EntityInsertedEvent<OrderItemsStatus>>,
    IConsumer<EntityUpdatedEvent<OrderItemsStatus>>
{
    private readonly IOrderService _orderService;
    private readonly INotifier _notifier;

    public OrderItemsStatusChangedConsumer(
        IOrderService orderService, INotifier notifier)
    {
        _orderService = orderService;
        _notifier = notifier;
    }

    public async Task HandleEventAsync(EntityInsertedEvent<OrderItemsStatus> eventMessage)
    {
        await InternalHandleEventAsync(eventMessage.Entity);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<OrderItemsStatus> eventMessage)
    {
        await InternalHandleEventAsync(eventMessage.Entity);
    }

    private async Task InternalHandleEventAsync(OrderItemsStatus orderItemsStatus)
    {
        //do nothing if just created
        var keys = orderItemsStatus.StatusDictionary.Keys;
        if(keys.Count() == 1 && keys.First() == KmConsts.OrderStatuses.Pending) 
            return;

        var orderId = orderItemsStatus.OrderId;
        var order = await _orderService.GetOrderByIdAsync(orderId);
        //return if order not exist or cancelled
        if (order == null || order.OrderStatus == OrderStatus.Cancelled)
            return;
        
        //publish to buyer
        await _notifier.NotifyCustomerOnOrderStausChangedAsync(order);
    }
}
