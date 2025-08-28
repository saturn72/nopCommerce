namespace KedemMarket.Domain.Agent;
public class CustomerAgentSession : BaseEntity
{
    public string AgentId { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}
