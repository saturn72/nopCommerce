using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;

namespace KedemMarket.Controllers;

[Route("api/analytics")]
[AllowAnonymous]
public class AnalyticsController : KmApiControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IExternalUsersService _externalUsersService;
    private readonly IWorkContext _workContext;
    private readonly TimeProvider _timeProvider;

    public AnalyticsController(
        IAnalyticsService analyticsService,
        IExternalUsersService externalUsersService,
        IWorkContext workContext,
        TimeProvider timeProvider)
    {
        _analyticsService = analyticsService;
        _externalUsersService = externalUsersService;
        _workContext = workContext;
        _timeProvider = timeProvider;
    }


    [HttpPost]
    public async Task<IActionResult> ReportEventAsync([FromBody] EventDataModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var customer = await _workContext.GetCurrentCustomerAsync();
        var map = customer != default ? await _externalUsersService.GetUserIdCustomerMapByInternalCustomerId(customer.Id) : default;

        var ed = new EventData
        {
            EventName = model.EventName,
            PerformedByNopUserId = customer?.Id,
            PerformedByKMUserId = map?.KmUserId,
            PerformedOnUtc = _timeProvider.GetUtcNow().DateTime
        };

        if (model.Data != default)
        {
            var jso = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                WriteIndented = false,
            };
            ed.Data = System.Text.Json.JsonSerializer.Serialize(model.Data, jso);
        }

        await _analyticsService.ReportEventAsync(ed);

        return Accepted();
    }
}
