using System.Text.Json;
using KedemMarket.Domain.Ordering;

namespace KedemMarket.Services.Vendor;

public class KmVendorService : IKmVendorService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<OrderItemsStatus> _orderItemStateRepository;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    private static readonly List<int> _openOrderStastusIds = [
        (int)OrderStatus.Pending,
        (int)OrderStatus.Processing];
    public KmVendorService(
        IRepository<Order> orderRepository,
        IRepository<OrderItem> orderItemRepository,
        IRepository<Product> productRepository,
        IRepository<OrderItemsStatus> orderItemStateRepository)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
        _orderItemStateRepository = orderItemStateRepository;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public Task<int> GetVendorOpenOrdersCountAsync(int vendorId)
    {
        var query = _orderRepository.Table;
        query = from o in query
                join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                join p in _productRepository.Table on oi.ProductId equals p.Id
                where p.VendorId == vendorId
                select o;
        query = query.Where(o => _openOrderStastusIds.Contains(o.OrderStatusId));
        return query.CountAsync();
    }
    public async Task SetOrdersItemStatusAsync(Order order, List<int> orderItemIds, string status)
    {
        var lcStatus = status.Trim().ToLowerInvariant();
        //how to use another table id?
        var ois =await (from o in _orderItemStateRepository.Table
                   where o.OrderId == order.Id
                   select o).FirstOrDefaultAsync();
        var oIIds = orderItemIds.Distinct().ToList();

        if (ois == null)
        {
            var d = new Dictionary<string, IEnumerable<int>>(StringComparer.OrdinalIgnoreCase)
                {
                    {lcStatus, oIIds}
                };

            ois = new OrderItemsStatus
            {
                OrderId = order.Id,
                Statuses = JsonSerializer.Serialize(d),
            };
            await _orderItemStateRepository.InsertAsync(ois);
            return;
        }

        var statuses = JsonSerializer.Deserialize<Dictionary<string, List<int>>>(ois.Statuses, _jsonSerializerOptions)
            ?? new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
        //set the statuses
        if (statuses.TryGetValue(lcStatus, out var existingStatus))
        {
            var temp = existingStatus.ToList();
            temp.AddRange(oIIds);
            statuses[lcStatus] = temp.Distinct().ToList();
        }
        else
        {
            statuses[lcStatus] = oIIds;
        }

        //clean old statuses + distinction
        foreach (var s in statuses.Keys)
        {
            if (s == lcStatus)
                continue;

            var d = new List<int>();
            foreach (var v in statuses[s])
                if (!oIIds.Contains(v) && !d.Contains(v))
                    d.Add(v);

            statuses[s] = d;
        }
        ois.Statuses = JsonSerializer.Serialize(statuses, _jsonSerializerOptions);
        await _orderItemStateRepository.UpdateAsync(ois);

    }
}