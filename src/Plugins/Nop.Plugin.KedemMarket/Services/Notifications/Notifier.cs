using KedemMarket.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace KedemMarket.Services.Notifications;

public class Notifier : INotifier
{
    private readonly IHubContext<NotificationHub> _notifucationHub;
    private readonly IExternalUsersService _externalUsersService;
    private readonly IOrderService _orderService;
    private readonly IMediaManager _mediaConvertor;
    private readonly IProductService _productService;
    private readonly IPictureService _pictureService;
    private static readonly Random _random = new();

    public Notifier(
        IHubContext<NotificationHub> notifucationHub,
        IExternalUsersService externalUsersService,
        IOrderService orderService,
        IProductService productService,
        IPictureService pictureService,
        IMediaManager mediaConvertor)
    {
        _notifucationHub = notifucationHub;
        _externalUsersService = externalUsersService;
        _orderService = orderService;
        _productService = productService;
        _pictureService = pictureService;
        _mediaConvertor = mediaConvertor;
    }

    public async Task NotifyCustomerOnOrderStausChangedAsync(Order order)
    {
        if (order == null || order.CustomerId == default)
            return;
        var map = await _externalUsersService.GetUserIdCustomerMapByNopCustomerId(order.CustomerId);
        var imageUrl = await GetOrderRandomItemImageUrl(order.Id);

        await _notifucationHub.Clients.All.SendAsync("IncomingMessage", new
        {
            key = "order-status-changed",
            payload = new
            {
                imageUrl,
                orderId = order.Id,
                userIds = new[] { map.KmUserId }
            },
        });
    }

    public async Task OnNewOrderAsync(string userId, IEnumerable<int> vendorIds, int orderId)
    {
        if (orderId <=0 || vendorIds == null || !vendorIds.Any())
            return;

        var imageUrl = await GetOrderRandomItemImageUrl(orderId);
        await _notifucationHub.Clients.All.SendAsync("IncomingMessage", new
        {
            key = "order-new",
            payload = new
            {
                userId,
                imageUrl,
                vendorIds = vendorIds.ToArray(),
                orderId
            },
        });
    }
    private async Task<string> GetOrderRandomItemImageUrl(int orderId)
    {
        var items = await _orderService.GetOrderItemsAsync(orderId);

        //picture
        var i = items.Count > 1 ? _random.Next(0, items.Count - 1) : 0;
        var orderItemForImage = items?.ElementAtOrDefault(i);
        var product = await _productService.GetProductByIdAsync(orderItemForImage.ProductId);
        if (product != null)
        {
            var orderItemPicture = await _pictureService.GetProductPictureAsync(product, orderItemForImage.AttributesXml);
            if (orderItemPicture != null)
                return await _mediaConvertor.GetThumbnailDownloadLink(orderItemPicture.Id);
        }
        return null;
    }
}
