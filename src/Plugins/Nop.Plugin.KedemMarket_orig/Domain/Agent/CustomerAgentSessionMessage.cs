namespace KedemMarket.Domain.Agent;

public class CustomerAgentSessionMessage : BaseEntity
{
    public int CustomerAgentSessionId { get; set; }
    public string MessageSource { get; set; }
    public string Content { get; set; }
    public DateTime UtcTimestamp { get; set; }
}