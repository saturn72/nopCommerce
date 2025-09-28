using KedemMarket.Models.Agent;

namespace KedemMarket.Controllers;

[Route("api/agent")]
public class AgentController : KmApiControllerBase
{
    private readonly IWorkContext _workContext;
    private readonly IAgentSessionService _agentService;

    public AgentController(IWorkContext workContext, IAgentSessionService agentService)
    {
        _workContext = workContext;
        _agentService = agentService;
    }

    [HttpGet("{agentId}")]
    public async Task<IActionResult> GetCustomerAgentInfoAsync(string? agentId)
    {
        if (agentId.IsNullOrEmpty())
            return BadRequest();

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == default)
            return BadRequest();

        var session = await _agentService.GetCustomerAgentSessionsAsync(customer, agentId);
        if (session == default)
            return Ok();

        var messages = await _agentService.GetCustomerAgentSessionMessagesAsync(session);

        var modelMessages = messages == default ?
        Enumerable.Empty<AgentSessionMessageModel>()
            : messages.Select(m => new AgentSessionMessageModel
            {
                Id = m.Id,
                MessageSource = m.MessageSource,
                Content = m.Content,
                UtcTimestamp = m.UtcTimestamp,
            }).ToList();
        var result = new AgentSessionInfoModel
        {
            Id = session.Id,
            AgentId = session.AgentId,
            CreatedOnUtc = session.CreatedOnUtc,
            Messages = modelMessages,
        };

        return ToJsonResult(result);
    }
}
