using KedemMarket.Domain.Ordering;
using KedemMarket.Services.Notifications;
using KedemMarket.Services.Vendor;

namespace KedemMarket.Consumers;
public class KmOrderPlacedConsumer : IConsumer<EntityInsertedEvent<KmOrder>>
{
    private readonly INotifier _notifier;
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IKmVendorService _kmVendorService;

    public KmOrderPlacedConsumer(
        INotifier notifier,
        IRepository<OrderItem> orderItemRepository,
        IRepository<Product> productRepository,
        IKmVendorService kmVendorService)
    {
        _notifier = notifier;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
        _kmVendorService = kmVendorService;
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
