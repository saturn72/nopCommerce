


using Nop.Web.Areas.Admin.Models.Orders;

namespace KedemMarket.Factories.Orders;

public class OrderModelFactory : IOrderModelFactory
{
    private readonly Nop.Web.Areas.Admin.Factories.IOrderModelFactory _orderModelFactory;
    private readonly MediaConvertor _mediaConvertor;
    private readonly IProductService _productService;

    public OrderModelFactory(
        Nop.Web.Areas.Admin.Factories.IOrderModelFactory orderModelFactory,
        MediaConvertor mediaConvertor,
        IProductService productService)
    {
        _orderModelFactory = orderModelFactory;
        _mediaConvertor = mediaConvertor;
        _productService = productService;
    }

    public async Task<IEnumerable<OrderModel>> PrepareOrderDetailsModelsAsync(IEnumerable<Order> orders)
    {
        var models = await orders.SelectAwait(async o => await _orderModelFactory.PrepareOrderModelAsync(null, o)).ToListAsync();
        foreach (var model in models)
        {
            foreach (var item in model.Items)
                item.PictureThumbnailUrl = await _mediaConvertor.GetDownloadLinkAsync(item.ProductId, KmConsts.MediaTypes.Thumbnail);
        }
        return models;
    }

    public async Task<IEnumerable<OrderModel>> PrepareOrderDetailsModelsByVendorIdAsync(IEnumerable<Order> orders, int vendorId)
    {
        var models = await PrepareOrderDetailsModelsAsync(orders);

        var productIds = models.SelectMany(m => m.Items.Select(i => i.ProductId)).Distinct().ToArray();
        if (productIds.Length == 0)
            return [];

        var allProducts = await _productService.GetProductsByIdsAsync(productIds);
        var res = new List<OrderModel>();
        foreach (var model in models)
        {
            var vendorItems = new List<OrderItemModel>();
            foreach (var item in model.Items)
            {
                var product = allProducts.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null || product.VendorId != vendorId)
                    continue;
                vendorItems.Add(item);
            }
            if (vendorItems.Count == 0)
                continue;

            model.Items = vendorItems;
            res.Add(model);
        }

        return res;
    }

}
