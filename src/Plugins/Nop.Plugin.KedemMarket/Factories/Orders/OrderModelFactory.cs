using KedemMarket.Services.Vendor;
using Nop.Web.Areas.Admin.Models.Orders;

namespace KedemMarket.Factories.Orders;

public class OrderModelFactory : IOrderModelFactory
{
    private readonly Nop.Web.Areas.Admin.Factories.IOrderModelFactory _orderModelFactory;
    private readonly IMediaManager _mediaConvertor;
    private readonly IProductService _productService;
    private readonly IKmVendorService _kmVendorService;

    public OrderModelFactory(
        Nop.Web.Areas.Admin.Factories.IOrderModelFactory orderModelFactory,
        IMediaManager mediaConvertor,
        IProductService productService,
        IKmVendorService kmVendorService)
    {
        _orderModelFactory = orderModelFactory;
        _mediaConvertor = mediaConvertor;
        _productService = productService;
        _kmVendorService = kmVendorService;
    }

    public async Task<IEnumerable<OrderModel>> PrepareOrderDetailsModelsAsync(IEnumerable<Order> orders)
    {
        var models = await orders.SelectAwait(async o => await _orderModelFactory.PrepareOrderModelAsync(null, o)).ToListAsync();
        foreach (var model in models)
        {
            foreach (var item in model.Items)
                item.PictureThumbnailUrl = await _mediaConvertor.GetThumbnailDownloadLink(item.ProductId);
        }
        return models;
    }

    public async Task<IEnumerable<VendorOrderModel>> PrepareVendorOrderModelsAsync(IEnumerable<Order> orders, Vendor vendor)
    {
        ThrowIfNull(orders, nameof(orders));
        ThrowIfNull(vendor, nameof(vendor));
        if (!orders.Any())
            return [];

        var models = await PrepareOrderDetailsModelsAsync(orders);

        var productIds = models.SelectMany(m => m.Items.Select(i => i.ProductId)).Distinct().ToArray();
        if (productIds.Length == 0)
            return [];

        var allProducts = await _productService.GetProductsByIdsAsync(productIds);
        var res = new List<VendorOrderModel>();
        foreach (var model in models)
        {
            var vendorItems = new List<VendorOrderItemModel>();
            var ois = await _kmVendorService.GetOrdersItemsStatusAsync(model.Id);
            foreach (var item in model.Items)
            {
                var product = allProducts.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null || product.VendorId != vendor.Id)
                    continue;

                var o = ois?.StatusDictionary.FirstOrDefault(k => k.Value.Contains(item.Id));
                var pp = await _productService.GetProductPicturesByProductIdAsync(product.Id);
                var picId = pp?.FirstOrDefault()?.PictureId;
                var ptu = picId.HasValue ? await _mediaConvertor.GetThumbnailDownloadLink(picId.Value) : null;

                var vi = new VendorOrderItemModel
                {
                    Id = item.Id,
                    AttributeInfo = item.AttributeInfo,
                    Quantity = item.Quantity,
                    PictureThumbnailUrl = ptu,
                    ProductId = item.Id,
                    ProductName = item.ProductName,
                    ItemOrderStatus = o?.Key,
                };
                vendorItems.Add(vi);
            }
            if (vendorItems.Count == 0)
                continue;
            var vom = new VendorOrderModel
            {
                Id = model.Id,
                CreatedOn = model.CreatedOn,
                OrderStatus = model.OrderStatus,
                OrderTotal = model.OrderTotal,
                Items = vendorItems,
            };
            var resOis = new List<string>();


            //var vom = new VendorOrderModel
            //{
            //    Items = vendorItems,
            //    OrderItemsStatuses = ois.Keys
            //};
            res.Add(vom);
        }

        return res;
    }

}
