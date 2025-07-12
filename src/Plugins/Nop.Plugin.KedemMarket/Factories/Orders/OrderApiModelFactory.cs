


namespace KedemMarket.Factories.Orders;

public class OrderApiModelFactory : IOrderApiModelFactory
{
    private readonly Nop.Web.Areas.Admin.Factories.IOrderModelFactory _orderModelFactory;
    private readonly MediaConvertor _mediaConvertor;

    public OrderApiModelFactory(
        Nop.Web.Areas.Admin.Factories.IOrderModelFactory orderModelFactory,
        MediaConvertor mediaConvertor)
    {
        _orderModelFactory = orderModelFactory;
        _mediaConvertor = mediaConvertor;
    }

    public async Task<IEnumerable<Nop.Web.Areas.Admin.Models.Orders.OrderModel>> PrepareOrderDetailsModelsAsync(IEnumerable<Order> orders)
    {
        var models = await orders.SelectAwait(async o => await _orderModelFactory.PrepareOrderModelAsync(null, o)).ToListAsync();
        foreach (var model in models)
        {
            foreach (var item in model.Items)
                item.PictureThumbnailUrl = await _mediaConvertor.GetDownloadLinkAsync(item.ProductId, KmConsts.MediaTypes.Thumbnail);
        }
        return models;
    }
}
