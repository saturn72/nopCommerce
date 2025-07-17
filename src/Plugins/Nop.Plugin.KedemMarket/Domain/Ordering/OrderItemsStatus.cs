namespace KedemMarket.Domain.Ordering;
public class OrderItemsStatus : BaseEntity
{
    public int OrderId { get; set; }
    public string Statuses { get; set; } 
}
