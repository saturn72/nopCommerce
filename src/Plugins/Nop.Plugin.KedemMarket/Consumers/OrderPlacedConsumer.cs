using KedemMarket.Services.Notifications;

namespace KedemMarket.Consumers;
public class OrderPlacedConsumer : IConsumer<EntityInsertedEvent<KmOrder>>
{
    private readonly INotifier _notifier;
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IRepository<Product> _productRepository;

    public OrderPlacedConsumer(
        INotifier notifier,
        IRepository<OrderItem> orderItemRepository,
        IRepository<Product> productRepository)
    {
        _notifier = notifier;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
    }
    public async Task HandleEventAsync(EntityInsertedEvent<KmOrder> eventMessage)
    {
        var orderId = eventMessage.Entity.NopOrderId;
        if (orderId == 0)
            return;

        var vendorIds = await (
                    from oi in _orderItemRepository.Table
                    join p in _productRepository.Table on oi.ProductId equals p.Id
                    where oi.OrderId == orderId && p.VendorId != 0
                    select p.VendorId
                ).Distinct().ToArrayAsync();

        if (vendorIds.Length == 0)
            return;

        await _notifier.NotifyVendorsOnNewOrderAsync(vendorIds);
    }
}
