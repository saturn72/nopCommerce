namespace KedemMarket.Domain.Analytics;
public class EventData : BaseEntity
{
    public string EventName { get; set; }
    public string? PerformedByKMUserId { get; set; }
    public int? PerformedByNopUserId { get; set; }
    public DateTime PerformedOnUtc { get; set; }
    public string? Data { get; set; }
}
