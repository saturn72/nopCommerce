
namespace KedemMarket.Models.Agent;
public record AgentSessionInfoModel : BaseNopEntityModel
{
    public string? AgentId { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public object?  Messages { get; init; }
}

public record AgentSessionMessageModel : BaseNopEntityModel
{
    public string? MessageSource { get; init; }
    public string? Content { get; init; }
    public DateTime UtcTimestamp { get; init; }
}
