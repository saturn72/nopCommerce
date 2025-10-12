namespace KedemMarket.Models.Analytics;
public record EventDataModel
{
    public string? EventName { get; init; }
    public Dictionary<string, object>? Data { get; init; }
}
