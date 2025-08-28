
using KedemMarket.Domain.Agent;

namespace KedemMarket.Services.Agent;
public interface IAgentSessionService
{
    public Task<CustomerAgentSession> GetCustomerAgentSessionsAsync(Customer customer, string agentId);
    public Task<IEnumerable<CustomerAgentSessionMessage>> GetCustomerAgentSessionMessagesAsync(CustomerAgentSession session);
}
