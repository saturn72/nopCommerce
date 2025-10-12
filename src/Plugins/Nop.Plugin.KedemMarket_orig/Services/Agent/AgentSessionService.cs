using KedemMarket.Domain.Agent;

namespace KedemMarket.Services.Agent;

public class AgentSessionService : IAgentSessionService
{
    private readonly IRepository<CustomerAgentSession> _userAgentSessionRepository;
    private readonly IRepository<CustomerAgentSessionMessage> _userAgentSessionMessageRepository;

    public AgentSessionService(
        IRepository<CustomerAgentSession> userAgentSessionRepository,
        IRepository<CustomerAgentSessionMessage> userAgentSessionMessageRepository)
    {
        _userAgentSessionRepository = userAgentSessionRepository;
        _userAgentSessionMessageRepository = userAgentSessionMessageRepository;
    }

    public async Task<IEnumerable<CustomerAgentSessionMessage>> GetCustomerAgentSessionMessagesAsync(CustomerAgentSession session)
    {
        return await (from m in _userAgentSessionMessageRepository.Table
                      where m.CustomerAgentSessionId == session.Id
                      select m).ToListAsync();
    }

    public async Task<CustomerAgentSession> GetCustomerAgentSessionsAsync(Customer customer, string agentId)
    {
        ThrowIfNull(customer);
        agentId.ThrowIfNullOrEmpty(nameof(agentId));

        var sessionIds = await (from s in _userAgentSessionRepository.Table
                                where s.CustomerId == customer.Id &&
                                s.AgentId == agentId
                                select s).FirstOrDefaultAsync();
        return sessionIds;

    }
}
